#if defined(cl_khr_fp64)
#  pragma OPENCL EXTENSION cl_khr_fp64: enable
#elif defined(cl_amd_fp64)
#  pragma OPENCL EXTENSION cl_amd_fp64: enable
#else
#  error double precision is not supported
#endif
kernel void AddWeighted(
	global const unsigned char *Src,
	global const float *Kern,
	global unsigned char *Dst,
	int rows,
	int cols,
	int anchX,
	int anchY,
	int kernRows,
	int kernCols,
	int channels
)
{
	size_t id = get_global_id(0);
	int offset = id%channels;
	int idC = id / 3;
	int y = (id) / (cols*channels);
	int x = ((id) % (cols*channels) )/channels;
	if (x < anchX || x >(cols - kernCols + anchX)
		||
		y < anchY || y > (rows - kernRows+anchY))
	{
		Dst[id] = 255; // border
	}
	else
	{
		double result = 0;
		for (int k = 0; k < kernRows; ++k)
		{
			for (int l = 0; l < kernCols; ++l)
			{
				//result += (kern.at<float>(k, l)* source_rows[i + k - anchor.x][j + (l - anchor.y)*source.channels()]);
				int disoff = ((x + l - anchX)*channels) % channels;
				result += Kern[k*kernCols + l] * Dst[(y + k - anchY)*cols*channels + (x+l- anchX)*channels+offset];
				//result += Kern[k*kernCols + l] * 
			}
		}
		//result /= (3 * 3);
		int r;
		if (result - (int)result > 0.5)
		{
			r = result + 1;
		}
		else
		{
			r = result;
		}
		Dst[id] = r;
		//Dst[(y)*cols*channels + (x)*channels+offset] = id;
	}
}