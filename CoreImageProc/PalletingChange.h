#pragma once
#include "AbstractChange.h"
#include "ImageProcessing.h"

class PalletingChange : public AbstractChange
{
public:
	virtual void apply(cv::Mat& source)
	{
		imp::palleting256(source);
	}
};