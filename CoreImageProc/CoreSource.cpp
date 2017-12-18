#include <opencv2\core.hpp>
#include <iostream>
#include <opencv2\highgui.hpp>
#include "opencv2/objdetect/objdetect.hpp"
#include "ImageProcess.h"
#include "ImageEditor.h"
#include "CudaProcessing.h"
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
			Mat img = imread("img.jpg");
			//Mat img = Mat::zeros(Size(3000, 3000), CV_8UC3);
			Mat kern = Mat::ones(Size(60, 60), CV_32F);
			kern /= 60*60;
			auto s = clock();
			gpu::filter2D(img, kern,Point(54,23),edit.GPUDevice);
			auto e = clock();
			cout << "CPU: " << (e - s) / 1000.0 << endl;
		}
		else if (act == "gray")
		{
			edit.toGrayscale();
		}
		else if (act == "cuda_change")
		{
			for (int i = 5000; i < 5005; i+=1)
			{
				cout << "Mat(" << i << "," << i << ")" << endl;
				{
					auto s = clock();
					Mat img = Mat::zeros(Size(i, i), CV_8UC3);
					//imp::changeContrastAndBrightness(img, 1, 0);
					auto e = clock();
					cout << "CPU: " << (e - s) / 1000.0 << endl;
				}
				{
					auto s = clock();
					Mat img = Mat::zeros(Size(i, i), CV_8UC1);
					gpu::contratAndBirghtness_cuda(img, 1, 0, edit.GPUDevice);
					auto e = clock();
					cout << "GPU: " << (e - s) / 1000.0 << endl;
				}
				cout << endl;

			}
		}
	}
	while (true);
	return 0;
}