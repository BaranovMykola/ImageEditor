#pragma once
#include "AbstractChange.h"

#include <opencv2\imgproc.hpp>

class FaceDetectionChange : public AbstractChange
{
public:
	FaceDetectionChange()
	{
	};

	FaceDetectionChange(std::vector<cv::Rect> _faces)
	{
		faces = _faces;
	}

	~FaceDetectionChange()
	{
	};

	void apply(cv::Mat& img)
	{
		for (auto i : faces)
		{
			cv::rectangle(img, i, cv::Scalar(0, 255, 0), (img.rows + img.cols)*0.05+1);
		}
	}

private:
	std::vector<cv::Rect> faces;
};