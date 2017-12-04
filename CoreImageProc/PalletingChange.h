#pragma once
#include "AbstractChange.h"
#include "ImageProcessing.h"

class PalletingChange : public AbstractChange
{
public:
	virtual void apply(cv::Mat& source)
	{
		imp::paletting256(source);
	}
};