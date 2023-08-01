using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Evo.Scm.Domain
{
    public abstract class ExtensibleEntity: Entity, IHasExtraProperties
    {
        public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }
        protected ExtensibleEntity()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ExtensibleObjectValidator.GetValidationErrors(
                this,
                validationContext
            );
        }
    }

    public abstract class ExtensibleEntity<TKey> : Entity<TKey>, IHasExtraProperties
    {
        public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }
        protected ExtensibleEntity()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ExtensibleObjectValidator.GetValidationErrors(
                this,
                validationContext
            );
        }
    }
}
