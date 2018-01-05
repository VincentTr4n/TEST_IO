using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_IO
{
	class Program
	{
		public static void Solve()
		{
			using (IOProcess inp = new IOProcess("txt1.txt"))
			{
				int t = inp.NextInt();
				while (t-- > 0)
				{
					int n = inp.NextInt();
					int k = inp.NextInt();
					var a = inp.NextIntArray(n);
					int[] c = new int[k + 1];
					for (int i = 0; i <= k; i++) c[i] = k + 1;
					c[0] = 0;
					for (int i = 0; i < n; i++)
						for (int j = 0; j <= k; j++)
						{
							if (j >= a[i] && a[i] + c[j - a[i]] < c[j]) c[j] = a[i] + c[j - a[i]];
						}
					int res = 0;
					for (int i = 0; i <= k; i++) if (c[i] != k + 1) res = c[i];
					IOProcess.Out(res);
				}
			}
		}
		public static void Main(string[] args)
		{
			TestRunTime.RunTimeHandler runtime = Solve;
			TestRunTime.Solustion(runtime);
			Console.ReadKey();
		}
	}
}
