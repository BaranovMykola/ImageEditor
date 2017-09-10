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
		ImageProc(System::String^ fileName);
		Image ^ convertMatToImage(const cv::Mat & opencvImage);
		System::Drawing::Image^ readOriginalWrapper(System::String^ fileName);
	private:
		CoreImgEditor* editor;
	};
}
