namespace OpenIoT.Lib.Web.Models
{
    public class PresetsCollection : List<Preset>
    {
        public PresetsCollection()
        {

        }

        public PresetsCollection(IEnumerable<Preset> presets)
        {
            this.AddRange(presets);
        }
    }
}
