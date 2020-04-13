namespace Main
{
	using System;
	using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
	using UnityEngine.UI;
	using OpenCvSharp;
	using OpenCvSharp.Demo;

	public class FaceRecognition : WebCamera
    {
        public TextAsset faces;
        public TextAsset eyes;
        public TextAsset shapes;

        private FaceProcessorLive<WebCamTexture> processor;

        void Awake()
        {
			base.Awake();
			base.forceFrontalCamera = true;

			Debug.Log(shapes.bytes);

			processor = new FaceProcessorLive<WebCamTexture>();
			processor.Initialize(faces.text, eyes.text, shapes.bytes);

			processor.DataStabilizer.Enabled = true;
			processor.DataStabilizer.Threshold = 2.0;
			processor.DataStabilizer.SamplesCount = 2;

			processor.Performance.Downscale = 256;
			processor.Performance.SkipRate = 0;
        }

		protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
		{
			processor.ProcessTexture(input, TextureParameters);
			processor.MarkDetected();
			output = Unity.MatToTexture(processor.Image, output);
			return true;
		}
	}
}
