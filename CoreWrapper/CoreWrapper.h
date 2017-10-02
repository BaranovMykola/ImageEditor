// CoreWrapper.h

#pragma once

#using <System.Drawing.dll>

#include "../CoreImageProc/ImageProcess.h"
#include "../CoreImageProc/ImageEditor.h"

using namespace System;
using namespace System::Drawing;

namespace CoreWrapper {

	enum Side{WIDTH, HEIGHT};

	public ref class ImageProc
	{
	public:
		ImageProc();
		Bitmap^ getSource();
		void loadImage(System::String^ file);
		void applyContrastAndBrightness(float contrast, int brightness);
	private:
		Bitmap^ ConvertMatToBitmap(cv::Mat matToConvert);
		Bitmap ^ convertMatToImage(const cv::Mat & opencvImage);
		ImageEditor* editor;
	};
}
