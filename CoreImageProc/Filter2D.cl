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
	int kernCols,
	int kernRows,
	int channels
)
{
	size_t id = get_global_id(0);
	int offset = id%channels;
	int idC = id / 3;
	int y = (id)/(cols*channels);
	int x = (id)% (cols*channels)/channels;
	if (x <= 900 && y <= 900)
	{
		Dst[id] = 255;
	}
	//}
}