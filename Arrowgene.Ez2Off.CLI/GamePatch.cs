using System.Collections.Generic;

namespace Arrowgene.Ez2Off.CLI
{
    public class GamePatch
    {
        public int Offset { get; }
        public byte Original { get; set; }
        public byte Patched { get; }
        public bool OriginalDefined { get; }

        public static List<GamePatch> CreateRevertPatches(List<GamePatch> patches)
        {
            List<GamePatch> reverted = new List<GamePatch>();
            foreach (GamePatch patch in patches)
            {
                reverted.Add(new GamePatch(patch, true));
            }

            return reverted;
        }

        public GamePatch(GamePatch patch, bool revert = false)
        {
            if (revert)
            {
                Offset = patch.Offset;
                Original = patch.Patched;
                Patched = patch.Original;
                OriginalDefined = patch.OriginalDefined;
            }
            else
            {
                Offset = patch.Offset;
                Original = patch.Original;
                Patched = patch.Patched;
                OriginalDefined = patch.OriginalDefined;
            }
        }

        public GamePatch(int offset, byte patched)
        {
            Offset = offset;
            Patched = patched;
            Original = 0;
            OriginalDefined = false;
        }

        public GamePatch(int offset, byte original, byte patched)
        {
            Offset = offset;
            Original = original;
            Patched = patched;
            OriginalDefined = true;
        }

    }
}