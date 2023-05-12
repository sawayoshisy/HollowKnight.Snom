using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Modding;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Snom
{
    public class Snom : Mod
    {
        private Texture2D _tex;

        public override string GetVersion() => "v1";

        public override void Initialize()
        {
            On.Climber.Awake += Awake;

            Assembly asm = Assembly.GetExecutingAssembly();
            string[] manifestResourceNames = asm.GetManifestResourceNames();
            foreach (string res in manifestResourceNames)
            {
                Log(res);
                using Stream s = asm.GetManifestResourceStream(res) ?? throw new InvalidOperationException();
                byte[] buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
                _tex = new Texture2D(2, 2);
                _tex.LoadImage(buffer, markNonReadable: true);
            }
        }

        private void Awake(On.Climber.orig_Awake orig, Climber self)
        {
            orig(self);

            self.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = _tex;
        }
    }
}