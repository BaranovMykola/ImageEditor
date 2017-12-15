#include <opencv2\core.hpp>
#include <iostream>
#include <opencv2\highgui.hpp>
#include "opencv2/objdetect/objdetect.hpp"
#include "ImageProcess.h"
#include "ImageEditor.h"
#include <ctime>

int main()
{
	std::string act;
	ImageEditor edit("img.jpg");
	do
	{
		cin >> act;
		if (act == "show")
		{
			imshow("", edit.preview);
			waitKey();
			destroyAllWindows();
		}
		else if (act == "change")
		{
			float c;
			int b;
			cin >> c >> b;
			edit.changeContrastAndBrightness(c, b);
		}
		else if (act == "rotate")
		{
			int a;
			cin >> a;
			edit.rotate(a);

		}
		else if (act == "resize")
		{
			float p;
			cin >> p;
			edit.resize(p);
		}
		else if (act == "restore")
		{
			int s;
			cin >> s;
			edit.restore(s);
		}
		else if (act == "apply")
		{
			edit.apply();
		}
		else if (act == "history")
		{
			edit.getPreviewIcons(0.5);
		}
		else if (act == "detect")
		{
			edit.detectFace();
		}
		else if (act == "bmp")
		{
			edit.palleting256();
		}
		else if (act == "filter")
		{
			Mat img = cv::imread("img.jpg");
			Mat kern = Mat::ones(Size(6, 7), CV_32F);
			kern /= 6*7;
			//edit.filter(kern, Point(3, 3));
			//imp::filter2D(img, kern,Point(3,3));
			auto s = clock();
			filter2D_cuda(img, kern, Point(3, 3));
			auto e = clock();
			cout << "Time: " << (e - s) / 1000.0 << endl;
		}
		else if (act == "gray")
		{
			edit.toGrayscale();
		}
	}
	while (true);
	return 0;
}