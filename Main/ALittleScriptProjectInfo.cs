
namespace ALittle
{
    public class ALittleScriptProjectInfo : ProjectInfo
    {
        public ALittleScriptProjectInfo(ABnfFactory factory, ABnf abnf, string path)
            : base(factory, abnf, path)
        {
        }

        public override void AddAnalysis(FileItem file_item)
        {
            var file = file_item.GetFile();
            if (file == null) return;

            var root = file.GetRoot() as ALittleScriptRootElement;
            if (root == null) return;

            ALittleScriptIndex.inst.AddRoot(root);
        }

        public override void RemoveAnalysis(FileItem file_item)
        {
            var file = file_item.GetFile();
            if (file == null) return;

            var root = file.GetRoot() as ALittleScriptRootElement;
            if (root == null) return;

            ALittleScriptIndex.inst.RemoveRoot(root);
        }
    }
}
