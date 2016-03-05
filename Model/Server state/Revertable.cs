using System;
using System.Collections.Generic;
using System.IO;

namespace Delver
{
    public interface IReverter
    {
        void Revert(Revertable state);
    }


    [Serializable]
    public class Revertable
    {
        [NonSerialized] private IReverter caller;

        private string lastRevertFile;

        public Revertable(IReverter caller)
        {
            this.caller = caller;
        }

        public void SaveState(string file = null)
        {
            if (file == null)
            {
                file = Path.GetTempPath() + "ocg_" +  Guid.NewGuid() + ".obj";
                lastRevertFile = file;
            }
            Serializer.WriteToBinaryFile(file, this);
        }

        public void RevertState(string file = null)
        {
            if (file != null)
            {
                var state = Serializer.ReadFromBinaryFile<Revertable>(file);
                caller.Revert(state);
            }
            else if (lastRevertFile != null)
            {
                var state = Serializer.ReadFromBinaryFile<Revertable>(lastRevertFile);
                caller.Revert(state);
                CleanState();
            }
        }

        public void CleanState()
        {
            if (File.Exists(lastRevertFile))
                File.Delete(lastRevertFile);
            lastRevertFile = null;
        }

        public void RevertState(string GameStartFile, Dictionary<int, List<string>> history)
        {
            var state = Serializer.ReadFromBinaryFile<Revertable>(GameStartFile);
            caller.Revert(state);
        }

        public void SetCaller(IReverter caller)
        {
            this.caller = caller;
        }
    }
}