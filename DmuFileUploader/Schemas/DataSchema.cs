//------------------------------------------------------------------------------
// <auto-generated>
//     Deze code is gegenereerd met een hulpprogramma.
//     Runtime-versie:4.0.30319.42000
//
//     Als u wijzigingen aanbrengt in dit bestand, kan dit onjuist gedrag veroorzaken wanneer
//     de code wordt gegenereerd.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.6.1590.0.
// 
namespace Schema {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class entities {
        
        private entitiesEntity[] entityField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entity")]
        public entitiesEntity[] entity {
            get {
                return this.entityField;
            }
            set {
                this.entityField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class entitiesEntity {
        
        private entitiesEntityField[] fieldsField;
        
        private entitiesEntityRelationship[] relationshipsField;
        
        private string nameField;
        
        private string displaynameField;
        
        private ushort etcField;
        
        private string primaryidfieldField;
        
        private string primarynamefieldField;
        
        private bool disablepluginsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("field", IsNullable=false)]
        public entitiesEntityField[] fields {
            get {
                return this.fieldsField;
            }
            set {
                this.fieldsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("relationship", IsNullable=false)]
        public entitiesEntityRelationship[] relationships {
            get {
                return this.relationshipsField;
            }
            set {
                this.relationshipsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayname {
            get {
                return this.displaynameField;
            }
            set {
                this.displaynameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort etc {
            get {
                return this.etcField;
            }
            set {
                this.etcField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string primaryidfield {
            get {
                return this.primaryidfieldField;
            }
            set {
                this.primaryidfieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string primarynamefield {
            get {
                return this.primarynamefieldField;
            }
            set {
                this.primarynamefieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool disableplugins {
            get {
                return this.disablepluginsField;
            }
            set {
                this.disablepluginsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class entitiesEntityField {
        
        private string displaynameField;
        
        private string nameField;
        
        private string typeField;
        
        private bool customfieldField;
        
        private bool customfieldFieldSpecified;
        
        private string lookupTypeField;
        
        private bool primaryKeyField;
        
        private bool primaryKeyFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayname {
            get {
                return this.displaynameField;
            }
            set {
                this.displaynameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool customfield {
            get {
                return this.customfieldField;
            }
            set {
                this.customfieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool customfieldSpecified {
            get {
                return this.customfieldFieldSpecified;
            }
            set {
                this.customfieldFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lookupType {
            get {
                return this.lookupTypeField;
            }
            set {
                this.lookupTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool primaryKey {
            get {
                return this.primaryKeyField;
            }
            set {
                this.primaryKeyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool primaryKeySpecified {
            get {
                return this.primaryKeyFieldSpecified;
            }
            set {
                this.primaryKeyFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class entitiesEntityRelationship {
        
        private string nameField;
        
        private bool manyToManyField;
        
        private string relatedEntityNameField;
        
        private string m2mTargetEntityField;
        
        private string m2mTargetEntityPrimaryKeyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool manyToMany {
            get {
                return this.manyToManyField;
            }
            set {
                this.manyToManyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string relatedEntityName {
            get {
                return this.relatedEntityNameField;
            }
            set {
                this.relatedEntityNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string m2mTargetEntity {
            get {
                return this.m2mTargetEntityField;
            }
            set {
                this.m2mTargetEntityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string m2mTargetEntityPrimaryKey {
            get {
                return this.m2mTargetEntityPrimaryKeyField;
            }
            set {
                this.m2mTargetEntityPrimaryKeyField = value;
            }
        }
    }
}
