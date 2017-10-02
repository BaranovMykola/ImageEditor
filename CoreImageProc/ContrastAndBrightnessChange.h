#pragma once
#include "AbstractChange.h"
class ContrastAndBrightnessChange : public AbstractChange
{
public:
	ContrastAndBrightnessChange(float _contrast, int _brightness)
	{
		contrast = _contrast;
		brightness = _brightness;
	}
	void apply(cv::Mat& source)
	{
		source.convertTo(source, source.type(), contrast, brightness);
	}
private:
	float contrast;
	int brightness;
};