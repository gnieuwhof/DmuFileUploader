namespace DmuFileUploader
{
    using Data;
    using DmuFileUploader.ODataFilter;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class MainFrm : Form
    {
        private readonly HttpClient httpClient;

        private ConnectionInfo connectionInfo;

        private string file;


        public MainFrm(HttpClient httpClient)
        {
            this.httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));

            InitializeComponent();

            this.Text = Program.Title;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Login();
        }

        private DialogResult Login()
        {
            this.WriteLine("Login");

            using (var login = new LoginFrm(this,
                this.httpClient, this.connectionInfo))
            {
                DialogResult result = login.ShowDialog();

                if (result == DialogResult.OK)
                {
                    this.connectionInfo = login.Info;

                    string uri = this.connectionInfo.Resource.AbsoluteUri;
                    string username = this.connectionInfo.Username;
                    DateTime validTo = this.connectionInfo.ValidTo;

                    this.SetStatus($"Connected to {uri}");

                    this.WriteLine();
                    this.WriteLine("Connected");
                    this.WriteLine($"Host: {uri}");
                    this.WriteLine($"User: {username}");
                    this.WriteLine($"Expires: {validTo}");
                    this.WriteLine();
                }
                else
                {
                    this.WriteLine("Login cancelled");
                    this.WriteLine();
                }

                return result;
            }
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WriteLine("Select file");

            var openFileDialog = new OpenFileDialog
            {
                Filter = "Configuration Migration Utility file (*.zip)|*.zip",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.file = openFileDialog.FileName;

                this.WriteLine($"Selected file: {this.file}");
                this.WriteLine();

                if (this.connectionInfo == null)
                {
                    Login();

                    if(this.connectionInfo == null)
                    {
                        return;
                    }
                }

                if(this.connectionInfo != null)
                {
                    this.WriteLine("");
                    this.WriteLine("BUSINESS!!");
                    this.WriteLine("BUSINESS!!");
                    this.WriteLine("BUSINESS!!");
                    this.WriteLine("BUSINESS!!");
                    this.WriteLine("BUSINESS!!");
                    this.WriteLine("");
                }
            }
            else
            {
                this.WriteLine("Selecting file cancelled");
                this.WriteLine();
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            var startText = new[]
            {
                Program.Title,
                "",
                "This tool can be used to upload a file created with the " +
                "\"Common Data Service Configuration Migration\" tool from the Dynamics 365 SDK.",
                "",
                "NOTE!!!",
                "The primary key of the records will not be changed.",
                "This could decrease performace.",
                ""
            };

            string text = string.Join(Environment.NewLine, startText);

            this.Write(text);

            this.SetStatus("Not connected");
        }

        public void Write(string text)
        {
            this.TextTxt.AppendText(text);
        }

        public void WriteLine(string line = "")
        {
            string text = Environment.NewLine;

            if (!string.IsNullOrWhiteSpace(line))
            {
                string time = DateTime.Now.ToString("HH:mm:ss");

                text += $"{time} {line}";
            }

            this.Write(text);
        }

        private void SetStatus(string status)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");

            this.statusLabel.Text = $"{time} Status: {status}";
        }

        private async void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dataFile = this.file;

            string tempFolder = $"{Guid.NewGuid()}";

            ZipFile.ExtractToDirectory(dataFile, tempFolder);

            var schema = FileHelper.DeserializeXmlFile<Schema.entities>(
                Path.Combine(tempFolder, "data_schema.xml"));

            var data = FileHelper.DeserializeXmlFile<Data.entities>(
                Path.Combine(tempFolder, "data.xml"));

            AuthenticationHeaderValue authHeader = this.connectionInfo.AuthHeader;
            Uri endpointUri = new Uri(this.connectionInfo.Resource, "api/data/v9.2/");

            using (var httpClient = new ODataHttpClient(endpointUri, authHeader))
            {
                var oDataClient = new ODataClient(httpClient);

                try
                {
                    foreach (Schema.entitiesEntity entity in schema.entity)
                    {
                        var entityDefinition = await GetEntityDefinition(oDataClient, entity.name);

                        string resourcePath = entityDefinition.ResourcePath;

                        foreach(entitiesEntity entitiesEntity in data.entity)
                        {
                            entitiesEntityRecord[] records = entitiesEntity.records;

                            IEnumerable<Guid> ids = records.Select(r => new Guid(r.id));

                            IEnumerable<string> existingIds = await GetIds(
                                oDataClient, resourcePath, entity.primaryidfield, ids);

                            foreach (entitiesEntityRecord record in records)
                            {
                                try
                                {
                                    bool update = existingIds.Contains(record.id);

                                    this.WriteLine($"{(update ? "Updating" : "Inserting")} {entity.displayname} record with ID: {record.id}");

                                    string json = await ToJson(oDataClient, entityDefinition, entity, record);

                                    HttpResponseMessage response = update
                                        ? await oDataClient.PatchAsync(resourcePath, json, new Guid(record.id))
                                        : await oDataClient.PostAsync(resourcePath, json);

                                    if (!response.IsSuccessStatusCode)
                                    {
                                        string content = await response.Content.ReadAsStringAsync();
                                        this.WriteLine(content);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    this.WriteLine("Exception!");
                                    this.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.WriteLine(ex.ToString());
                }
            }
        }



        private static async Task<IEnumerable<string>> GetIds(ODataClient oDataClient,
            string resourcePath, string idField, IEnumerable<Guid> ids)
        {
            var result = new List<string>();

            IEnumerable<IEnumerable<Guid>> batches = ids.Batch(50);

            foreach (IEnumerable<Guid> batch in batches)
            {
                var primarykeyFilter = new ODataInFilter<Guid>(idField, batch);

                HttpResponseMessage response =
                    await oDataClient.GetAsync(resourcePath, primarykeyFilter, idField);

                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(content);
                }

                var idDict = JsonConvert.DeserializeObject<
                    ODataResponse<Dictionary<string, object>>>(content);

                IEnumerable<string> existingIds = idDict.Value
                    .Select(v => (string)v[idField]);

                result.AddRange(existingIds);
            }

            return result;
        }


        private static async Task<string> ToJson(
            ODataClient oDataClient,
            EntityDefinition entityDefinition,
            Schema.entitiesEntity entity,
            entitiesEntityRecord record
            )
        {
            var dict = new Dictionary<string, object>();

            foreach (entitiesEntityRecordField field in record.field)
            {
                if (field.value == null)
                {
                    continue;
                }

                Schema.entitiesEntityField fieldInfo =
                    entity.fields.FirstOrDefault(f => f.name == field.name);

                if (fieldInfo == null)
                {
                    continue;
                }

                object val = null;

                string fieldName = field.name;

                switch (fieldInfo.type)
                {
                    case "string":
                        val = $"{field.value}";
                        break;

                    case "guid":
                        val = new Guid(field.value);
                        break;

                    case "decimal":
                    case "money":
                        val = ConvertTo<decimal>(field.value);
                        break;

                    case "entityreference":
                        ManyToOneRelationship relation = entityDefinition.ManyToOneRelationships
                            ?.FirstOrDefault(r => r.ReferencedEntity == field.lookupentity && r.ReferencingAttribute == fieldName);
                        string navigationProperty = relation?.ReferencingEntityNavigationPropertyName;
                        fieldName = $"{navigationProperty}@odata.bind";
                        string resourcePath = await GetResourcePath(oDataClient, field.lookupentity);
                        string id = field.value;
                        val = $"/{resourcePath}({id})";
                        break;

                    case "datetime":
                        val = Convert.ToDateTime(field.value);
                        break;

                    case "float":
                        val = ConvertTo<double>(field.value);
                        break;

                    case "state":
                    case "number":
                    case "optionsetvalue":
                        val = ConvertTo<int>(field.value);
                        break;

                    case "bool":
                        if (!field.value.Equals("null", StringComparison.OrdinalIgnoreCase))
                        {
                            val = Convert.ToBoolean(field.value);
                        }
                        break;

                    case "optionsetvaluecollection":
                        string noBrackets = field.value.Trim('[', ']');
                        string[] parts = noBrackets.Split(',').Where(i => i != "-1").ToArray();
                        val = string.Join(",", parts);
                        break;

                    default:
                        break;
                }

                dict.Add(fieldName, val);
            }

            string json = JsonConvert.SerializeObject(dict);

            return json;
        }

        private static T ConvertTo<T>(string val)
        {
            val = val.Replace(',', '.');
            object result;

            if (typeof(T) == typeof(int))
            {
                result = Convert.ToInt32(val, CultureInfo.InvariantCulture);
            }
            else if (typeof(T) == typeof(double))
            {
                result = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            }
            else if (typeof(T) == typeof(decimal))
            {
                result = Convert.ToDecimal(val, CultureInfo.InvariantCulture);
            }
            else
            {
                throw new InvalidOperationException(
                    $"The type {typeof(T)} is not supported");
            }

            return (T)result;
        }

        private static async Task<string> GetResourcePath(
            ODataClient oDataClient, string entityName)
        {
            var entityDefinition = await GetEntityDefinition(oDataClient, entityName);

            string resourcePath = entityDefinition.ResourcePath;

            return resourcePath;
        }

        private static async Task<EntityDefinition> GetEntityDefinition(
            ODataClient oDataClient, string entityName)
        {
            var setNameFilter = new ODataEqualsFilter<string>("LogicalName", entityName);

            var expand = new ODataExpand("ManyToOneRelationships",
                "ReferencedEntity", "ReferencingAttribute", "ReferencingEntityNavigationPropertyName");

            var entityDefinition = await oDataClient.FindEntryAsync<EntityDefinition>(
                "EntityDefinitions", setNameFilter, expand, "EntitySetName");

            return entityDefinition;
        }
    }
}
