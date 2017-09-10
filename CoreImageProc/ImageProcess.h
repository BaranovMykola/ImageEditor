#pragma once
#include <opencv2\core.hpp>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>

#include <string>

using namespace std;
using namespace cv;

class CoreImgEditor
{
public:
	CoreImgEditor(string fileName, int upperWidth, int upperHeight)
	{
		initilizeParamsByDefault();
		loadImg(fileName);
		reducedSource = reduceImg(source, upperWidth, upperHeight);
	}
	CoreImgEditor()
	{
		initilizeParamsByDefault();
	}

	void loadImg(std::string fileName)
	{
		source = imread(fileName);
		//source = Mat();
	}

	cv::Mat RotateAt(const cv::Mat img)
	{
			//Computing affine matrix
			auto center = Point2f(img.cols / 2, img.rows / 2);
			cv::Mat rotateMat = getRotationMatrix2D(center, rotateAngle, 1);
			auto rotRect = RotatedRect(center, img.size(), rotateAngle).boundingRect();
			rotateMat.at<double>(0, 2) += rotRect.width / 2.0 - center.x;
			rotateMat.at<double>(1, 2) += rotRect.height / 2.0 - center.y;
		
			cv::Mat rotatedImg;
			warpAffine(img, rotatedImg, rotateMat, rotRect.size(), 1, BORDER_TRANSPARENT);
			return rotatedImg;
	}

	cv::Mat resizeImg(const cv::Mat& img)
	{
			cv::Size newSize(img.size().width*percentRatio, img.size().height*percentRatio);
			cv::Mat resized = Mat::zeros(newSize, CV_8UC3);
			cv::resize(img, resized, newSize);
			return resized;
	}

	cv::Mat contrastAndBrightness(const cv::Mat& img)
	{
			Mat edited;
			img.convertTo(edited, edited.type(), contrast, brightness);
			return edited;
	}

	void changeContrastAndBrightness(float _contrast, int _brightness)
	{
		contrast = _contrast;
		brightness = _brightness;
		editSource(reducedSource);
	}

	void resize(float _percentRatio)
	{
		percentRatio = _percentRatio;
		editSource(reducedSource);
	}

	void rotate(float _rotateAngle)
	{
		rotateAngle = _rotateAngle;
		editSource(reducedSource);
	}

	void editImage(float _sizeRatio, float _rotateAngle, float _contrast, int _brightness)
	{
		percentRatio = _sizeRatio;
		rotateAngle = _rotateAngle;
		contrast = _contrast;
		brightness = _brightness;
		editSource(reducedSource);
	}

	void updatePreview(int width, int height)
	{
		if (changed.empty())
		{
			changed = reducedSource;
		}
		preview = reduceImg(changed, width*percentRatio, height*percentRatio);
	}

	void save(string fileName)
	{
		editSource(source);
		imwrite(fileName, changed);
	}

	cv::Mat getPreview()
	{
		return preview;
	}

private:

	void editSource(const cv::Mat& img)
	{
		changed = contrastAndBrightness(img);
		changed = RotateAt(changed);
		changed = resizeImg(changed);
	}
	void initilizeParamsByDefault()
	{
		percentRatio = 1;
		contrast = 1;
		brightness = 0;
		rotateAngle = 0;
	}
	cv::Mat reduceImg(const cv::Mat& img, int upperWidth, int upperHeight)
	{
		Mat reduced = img.clone();
		while (reduced.size().width >= upperWidth || reduced.size().height >= upperHeight)
		{
			Mat scaledDown;
			pyrDown(reduced, reduced);
		}
		return reduced;
	}

	cv::Mat source;
	cv::Mat reducedSource;
	cv::Mat changed;
	cv::Mat preview;

	float rotateAngle;
	float contrast;
	int brightness;
	float percentRatio;
};