// CoreWrapper.h

#pragma once

#using <System.Drawing.dll>

using namespace System;
using namespace System::Drawing;

namespace CoreWrapper {

	enum Side{WIDTH, HEIGHT};

	public ref class ImageProc
	{

	public:
		Image ^ convertMatToImage(const cv::Mat & opencvImage);
		System::Drawing::Image^ readOriginalWrapper(System::String^ fileName);
	};
}
