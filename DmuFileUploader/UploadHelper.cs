namespace DmuFileUploader
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class UploadHelper
    {
        public UploadHelper(
            ODataClient oDataClient,
            Schema.entitiesEntity entity,
            EntityDefinition entityDefinition,
            string resourcePath,
            IEnumerable<string> existingIds
            )
        {
            this.Client = oDataClient ??
                throw new ArgumentNullException(nameof(oDataClient));

            this.Definition = entityDefinition ??
                throw new ArgumentNullException(nameof(entityDefinition));

            this.Entity = entity ??
                throw new ArgumentNullException(nameof(entity));

            this.ResourcePath = resourcePath ??
                throw new ArgumentNullException(nameof(resourcePath));

            this.ExistingIds = existingIds ??
                throw new ArgumentNullException(nameof(existingIds));
        }


        public ODataClient Client { get; }

        public EntityDefinition Definition { get; }

        public string ResourcePath { get; }

        public IEnumerable<string> ExistingIds { get; }

        public Schema.entitiesEntity Entity { get; }

        public ConcurrentDictionary<string, string> ResourcePathCache { get; } =
            new ConcurrentDictionary<string, string>();
    }
}
