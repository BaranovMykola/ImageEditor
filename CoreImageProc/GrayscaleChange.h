#pragma once
#include "AbstractChange.h"
#include "ImageProcessing.h"

class GrayscalseChange : public AbstractChange
{
public:
	void apply(cv::Mat& source)
	{
		imp::toGrayscale(source);
	}
};
