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
		Bitmap^ getPreview();
		System::Collections::Generic::List<Bitmap^>^ getPreivewIcons(float resizeRatio);
		void loadImage(System::String^ file);
		void applyContrastAndBrightness(float contrast, int brightness);
		void applyRotate(float angle);
		void applyResize(float ratio);
		int getMinimumOfImage();
		void apply();
		void restore(int changeIndex);
		void save(System::String^ fileName);
		void detectFace();
	private:
		Bitmap^ ConvertMatToBitmap(cv::Mat matToConvert);
		Bitmap ^ convertMatToImage(const cv::Mat & opencvImage);
		ImageEditor* editor;
	};
}
