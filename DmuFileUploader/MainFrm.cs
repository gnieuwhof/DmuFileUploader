namespace DmuFileUploader
{
    using Data;
    using DmuFileUploader.ODataFilter;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class MainFrm : Form
    {
        private const int BACTCH_SIZE = 10;

        private readonly HttpClient httpClient;

        private ConnectionInfo connectionInfo;

        private string file;

        private bool cancelUploading;


        public MainFrm(HttpClient httpClient)
        {
            ServicePointManager.DefaultConnectionLimit = BACTCH_SIZE;

            this.httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));

            InitializeComponent();

            this.Text = Program.Title;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(750, 500);

            var startText = new[]
            {
                Program.Title,
                "",
                "This tool can be used to upload files created with the " +
                "\"Common Data Service Configuration Migration\" tool from the Dynamics 365 SDK.",
                ""
            };

            string text = string.Join(Environment.NewLine, startText);

            this.Write(text);

            this.SetStatus();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Login();
        }

        private void SelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SelectFile();
        }

        private async void UploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.file))
            {
                this.SelectFile();

                if (!File.Exists(this.file))
                {
                    this.WriteLine("Upload cancelled, no file selected.");
                    return;
                }
            }

            if (this.connectionInfo == null)
            {
                this.Login();

                if (this.connectionInfo == null)
                {
                    this.WriteLine("Upload cancelled, not connected.");
                    return;
                }
            }

            var lines = new[]
            {
                "Upload file:",
                this.file,
                "",
                "To Dynamics 365 instance:",
                $"{this.connectionInfo.Resource }"
            };

            string message = string.Join(Environment.NewLine, lines);

            DialogResult result = MessageBox.Show(message, "Upload to Dynamics 365",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                await this.UploadFile();
            }
            else
            {
                this.WriteLine("Upload cancelled.");
            }
        }


        private DialogResult Login()
        {
            this.WriteLine("Opening login form.");

            using (var login = new LoginFrm(this,
                this.httpClient, this.connectionInfo))
            {
                DialogResult result = login.ShowDialog();

                if (result == DialogResult.OK)
                {
                    this.connectionInfo = login.Info;

                    string uri = this.connectionInfo.Resource.AbsoluteUri;
                    string username = this.connectionInfo.Username;
                    DateTime utcValidTo = this.connectionInfo.UtcValidTo;
                    DateTime localValidTo = utcValidTo.ToLocalTime();

                    this.SetStatus();

                    this.WriteLine();
                    this.WriteLine("Connection info:");
                    this.WriteLine($"Host: {uri}");
                    this.WriteLine($"User: {username}");
                    this.WriteLine($"Expires: {localValidTo}");
                    this.WriteLine();
                }
                else
                {
                    this.WriteLine("Login cancelled.");
                    this.WriteLine();
                }

                return result;
            }
        }

        private bool SelectFile()
        {
            this.WriteLine("Opening file selector.");

            var openFileDialog = new OpenFileDialog
            {
                Filter = "Configuration Migration Utility file (*.zip)|*.zip",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.file = openFileDialog.FileName;

                this.SetStatus();

                this.WriteLine($"Selected file: {this.file}");
                this.WriteLine();

                return true;
            }

            this.WriteLine("Selecting file cancelled.");
            this.WriteLine();

            return false;
        }

        public void Write(string text)
        {
            AddText(this.TextTxt, text);
        }
        private static void AddText(TextBox textBox, string text)
        {
            if (textBox.IsDisposed)
            {
                return;
            }

            var bottomLeft = new Point(3, textBox.ClientSize.Height - 3);
            int lastVisibleCharIndex = textBox.GetCharIndexFromPosition(bottomLeft);
            int lastVisibleLine = textBox.GetLineFromCharIndex(lastVisibleCharIndex);

            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;
            int topLeft = textBox.GetCharIndexFromPosition(new Point(3, 3));

            textBox.Suspend();

            bool restoreScollPosition =
                (textBox.Lines.Length - 1 >= lastVisibleLine);

            textBox.AppendText(text);

            if (restoreScollPosition)
            {
                textBox.Select(topLeft, 0);
                textBox.ScrollToCaret();
            }

            // Restore selection
            textBox.Select(selectionStart, selectionLength);

            textBox.Resume();
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

        private void SetStatus()
        {
            string connectedTo = (this.connectionInfo != null)
                ? this.connectionInfo?.Resource.AbsoluteUri
                : "not connected";

            string selectedFile = (this.file != null)
                ? Path.GetFileName(this.file)
                : "none";

            var items = new[]
            {
                $"Connection: {connectedTo}",
                $"Selected file: {selectedFile}"
            };

            string status = string.Join("    |    ", items);

            this.statusLabel.Text = status;
        }

        private async Task UploadFile()
        {
            this.cancelUploading = false;

            this.EnableToolStripItems(false);

            try
            {
                await InnerUploadFile();
            }
            catch (Exception ex)
            {
                this.WriteLine(ex.ToString());
            }
            finally
            {
                this.WriteLine();

                this.EnableToolStripItems(true);

                if (this.cancelUploading)
                {
                    this.WriteLine("Uploading cancelled.");
                    this.WriteLine();
                }
            }
        }

        private void EnableToolStripItems(bool uploading)
        {
            this.selectToolStripMenuItem.Enabled = uploading;
            this.uploadToolStripMenuItem.Enabled = uploading;
            this.cancelToolStripMenuItem.Enabled = !uploading;
            this.loginToolStripMenuItem.Enabled = uploading;
        }

        private async Task InnerUploadFile()
        {
            string tempFolder = $"{Guid.NewGuid()}";

            try
            {
                this.WriteLine($"Extracting zip file to temp folder: {tempFolder}");
                ZipFile.ExtractToDirectory(this.file, tempFolder);

                this.WriteLine("Deserializing data schema.");
                var schema = FileHelper.DeserializeXmlFile<Schema.entities>(
                    Path.Combine(tempFolder, "data_schema.xml"));

                this.WriteLine("Deserializing data.");
                var data = FileHelper.DeserializeXmlFile<entities>(
                    Path.Combine(tempFolder, "data.xml"));

                AuthenticationHeaderValue authHeader = this.connectionInfo.AuthHeader;
                Uri endpointUri = new Uri(this.connectionInfo.Resource, Program.ApiPath);

                using (var httpClient = new ODataHttpClient(endpointUri, authHeader))
                {
                    var oDataClient = new ODataClient(httpClient,
                        this.connectionInfo, this.WriteLine);

                    var entityDefinitionCache = new Dictionary<string, EntityDefinition>();

                    string ies = schema.entity.Length == 1 ? "y" : "ies";
                    this.WriteLine($"Found {schema.entity.Length} entit{ies} in the schema.");
                    ies = data.entity.Length == 1 ? "y" : "ies";
                    this.WriteLine($"Found {data.entity.Length} entit{ies} in the data.");
                    foreach (entitiesEntity entitiesEntity in data.entity)
                    {
                        if (this.cancelUploading)
                        {
                            return;
                        }

                        this.WriteLine();

                        Schema.entitiesEntity entity = schema.entity
                            .FirstOrDefault(e => e.name == entitiesEntity.name);

                        if (entity == null)
                        {
                            this.WriteLine($"Could not find the entity '{data.entity}' in the schema.");
                            continue;
                        }

                        this.WriteLine($"Getting resource path for entity: {entity.name}");
                        EntityDefinition entityDefinition = await GetEntityDefinition(
                            oDataClient, entity.name, entityDefinitionCache);

                        string resourcePath = entityDefinition.ResourcePath;
                        this.WriteLine($"Resource path: {resourcePath}");

                        entitiesEntityRecord[] records = entitiesEntity.records;
                        this.WriteLine($"Found {records.Length} records to process.");
                        this.WriteLine();

                        IEnumerable<Guid> ids = records.Select(r => new Guid(r.id));

                        IEnumerable<string> existingIds = await GetIds(
                            oDataClient, resourcePath, entity.primaryidfield, ids);

                        var helper = new UploadHelper(
                            oDataClient,
                            entity,
                            entityDefinition,
                            resourcePath,
                            existingIds
                            );

                        var batches = records.Batch(BACTCH_SIZE);

                        foreach (IEnumerable<entitiesEntityRecord> batch in batches)
                        {
                            if (this.cancelUploading)
                            {
                                return;
                            }

                            await ProcessBatch(helper, batch);
                        }


                        /*
                         * BEGIN M2M
                         */

                        var relationships = new List<ODataRelationship>();

                        foreach (var m2m in entitiesEntity.m2mrelationships)
                        {
                            EntityDefinition targetEntityDefinition = await GetEntityDefinition(
                                oDataClient, m2m.targetentityname, entityDefinitionCache);

                            foreach (string targetId in m2m.targetids)
                            {
                                var relationship = ODataRelationship.Create(
                                    this.connectionInfo.Resource,
                                    targetEntityDefinition.ResourcePath,
                                    m2m.m2mrelationshipname,
                                    m2m.sourceid,
                                    targetId
                                    );

                                relationships.Add(relationship);
                            }
                        }

                        this.WriteLine();
                        this.WriteLine($"Found {relationships.Count()} relationships to process.");
                        this.WriteLine();

                        var relationshipBatches = relationships.Batch(BACTCH_SIZE);

                        var taskList = new List<ConfiguredTaskAwaitable<HttpResponseMessage>>();
                        foreach (IEnumerable<ODataRelationship> relationshipBatch in relationshipBatches)
                        {
                            if (this.cancelUploading)
                            {
                                return;
                            }

                            await ProcessRelations(oDataClient, relationshipBatch);
                        }

                        /*
                         * END M2M
                         */


                        this.WriteLine();
                        this.WriteLine($"Done processing entity: {entity.name}.");
                    }

                    string fileName = Path.GetFileName(this.file);

                    this.WriteLine();
                    this.WriteLine($"Done processing file: {fileName}.");
                }
            }
            finally
            {
                this.WriteLine($"Removing temp folder: {tempFolder}");
                Directory.Delete(tempFolder, true);
            }
        }

        private async Task<EntityDefinition> GetEntityDefinition(
            ODataClient oDataClient,
            string entityName,
            Dictionary<string, EntityDefinition> cache
            )
        {
            if (!cache.ContainsKey(entityName))
            {
                EntityDefinition entityDefinition =
                    await GetEntityDefinition(oDataClient, entityName);

                cache.Add(entityName, entityDefinition);
            }

            EntityDefinition result = cache[entityName];

            return result;
        }

        private async Task ProcessBatch(UploadHelper helper,
            IEnumerable<entitiesEntityRecord> records)
        {
            var tasks = new List<Task>();

            foreach (entitiesEntityRecord record in records)
            {
                Task task = this.ProcessRecord(helper, record);

                tasks.Add(task);
            }

            foreach (Task task in tasks)
            {
                await task;
            }
        }

        private async Task ProcessRecord(
            UploadHelper helper, entitiesEntityRecord record)
        {
            try
            {
                bool update = helper.ExistingIds.Contains(record.id);

                string json = await ToJson(helper, record);

                HttpResponseMessage response = update
                    ? await helper.Client.PatchAsync(helper.ResourcePath, json, new Guid(record.id))
                    : await helper.Client.PostAsync(helper.ResourcePath, json);

                if (!response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    this.WriteLine($"Processing record failed ({response.ReasonPhrase}) {content}");
                }
                else
                {
                    this.WriteLine($"{(update ? "Updated" : "Inserted")} {helper.Entity.displayname} record with ID: {record.id}");
                }
            }
            catch (Exception ex)
            {
                this.WriteLine($"Exception: {ex.Message}");
            }
        }

        private async Task ProcessRelations(
            ODataClient oDataClient,
            IEnumerable<ODataRelationship> relationships)
        {
            var tasks = new List<Task<HttpResponseMessage>>();

            foreach (ODataRelationship relationship in relationships)
            {
                this.WriteLine($"Processing realtionship {relationship.SourceId} - {relationship.TargetId}");

                var task = oDataClient.PostAsync(relationship);

                tasks.Add(task);
            }

            int index = 0;
            foreach (Task<HttpResponseMessage> task in tasks)
            {
                HttpResponseMessage result = await task;

                if(!result.IsSuccessStatusCode)
                {
                    var relationship = relationships.ElementAt(index);

                    this.WriteLine();
                    this.WriteLine($"Error ({result.StatusCode}) {relationship.SourceId} - {relationship.TargetId}");
                    this.WriteLine(await result.Content.ReadAsStringAsync());
                    this.WriteLine();
                }

                ++index;
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
            UploadHelper helper, entitiesEntityRecord record)
        {
            var dict = new Dictionary<string, object>();

            foreach (entitiesEntityRecordField field in record.field)
            {
                if (field.value == null)
                {
                    continue;
                }

                Schema.entitiesEntityField fieldInfo =
                    helper.Entity.fields.FirstOrDefault(f => f.name == field.name);

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
                        string referencedEntity = (field.lookupentity != null)
                            ? field.lookupentity
                            : fieldInfo.lookupType;

                        ManyToOneRelationship relation = helper.Definition.ManyToOneRelationships
                            ?.FirstOrDefault(r => r.ReferencedEntity == referencedEntity && r.ReferencingAttribute == fieldName);
                        string navigationProperty = relation?.ReferencingEntityNavigationPropertyName;
                        fieldName = $"{navigationProperty}@odata.bind";

                        string resourcePath;
                        if (helper.ResourcePathCache.ContainsKey(referencedEntity))
                        {
                            resourcePath = helper.ResourcePathCache[referencedEntity];
                        }
                        else
                        {
                            resourcePath = await GetResourcePath(helper.Client, referencedEntity);
                            helper.ResourcePathCache.TryAdd(referencedEntity, resourcePath);
                        }

                        string id = field.value;

                        if (string.IsNullOrWhiteSpace(id))
                        {
                            continue;
                        }

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

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string question = "Are you sure you want to exit the application?";

            DialogResult result = MessageBox.Show(question,
                Program.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WriteLine();
            this.WriteLine("Cancelling upload...");
            this.WriteLine();

            this.cancelUploading = true;
        }
    }
}
