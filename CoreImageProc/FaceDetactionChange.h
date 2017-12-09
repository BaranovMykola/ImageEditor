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
		imp::printFaces(img, faces);
	}

private:
	std::vector<cv::Rect> faces;
};