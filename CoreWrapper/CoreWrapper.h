// CoreWrapper.h

#pragma once

#using <System.Drawing.dll>

#include "../CoreImageProc/ImageProcess.h"

using namespace System;
using namespace System::Drawing;

namespace CoreWrapper {

	enum Side{WIDTH, HEIGHT};

	public ref class ImageProc
	{
	public:
		ImageProc(System::String^ fileName, int processingWidth, int processingHeight);
		System::Drawing::Bitmap^ readOriginalWrapper(System::String^ fileName);
		void loadNewImage(System::String^ fileName);
		void editImage(float _sizeRatio, float _rotateAngle, float _contrast, int _brightness);
		void editContrastAndBrightness(float _contrast, int _brightness);
		void rotateImage(float _grad);
		void resizeImage(float _ratio);
		System::Drawing::Image^ getPreview(int width, int height);
		Bitmap^ ConvertMatToBitmap(cv::Mat matToConvert);

	private:
		Bitmap ^ convertMatToImage(const cv::Mat & opencvImage);
		CoreImgEditor* editor;
	};
}
