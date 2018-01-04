using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace TEST_IO
{
	public class IOProcess :IDisposable
	{
		private TextReader _reader;
		private readonly static TextWriter _out = Console.Out;
		private int _bufferSize;
		private char[] _buffer;
		private bool _closeReader;
		private int _length, _pos;            // length of input and current possition
		private bool _check
		{
			get
			{
				if (_pos < _length) return false;
				FillBuffer(false);
				return true;
			}
		}

		public IOProcess(TextReader reader, int bufferSize, bool closeReader)
		{
			_reader = reader;
			_bufferSize = bufferSize;
			_closeReader = closeReader;
			_buffer = new char[_bufferSize];

			FillBuffer(false);
		}

		public IOProcess(TextReader reader, bool closeReader) : this(reader, 1 << 16, closeReader) { }  // 1<<16 = 2^16 (defaul size of buffer)

		public IOProcess(string fileName) : this(new StreamReader(fileName, Encoding.Default), true) { }   // read in file -> should close TextReader

		public IOProcess() : this(Console.In, false) { }

		/// <summary>
		/// Get the current character
		/// </summary>
		/// <returns></returns>
		private char CurrentChar()
		{
			if (_pos < _length) return _buffer[_pos];
			FillBuffer(true);
			return _buffer[_pos];
		}

		//
		// Read all input and add in buffer (char array)
		private void FillBuffer(bool throwExOnCheck)
		{
			_length = _reader.Read(_buffer, 0, _bufferSize);
			if (throwExOnCheck && _check) throw new IOException("Can't read, beyond limits.....!");
			_pos = 0;
		}

		//
		// Used in method of read a number 
		private void SkipWhiteSpaces()
		{
			while (!_check && IsWhiteSpace(CurrentChar())) _pos++;
		}

		//
		// Used in method of read a line (string)
		private void SkipUntilNextLine()
		{
			while (!_check && IsEndOfLine(CurrentChar())) _pos++;
		}

		private static bool IsWhiteSpace(char c) => c == ' ' || c == '\t' || c == '\n' || c == '\r';

		private static bool IsEndOfLine(char c) => c == '\n' || c == '\r';

		/// <summary>
		/// Read a character
		/// </summary>
		/// <returns></returns>
		private char NextChar()
		{
			if (_pos < _length) return _buffer[_pos++];
			FillBuffer(true);
			return _buffer[_pos++];
		}
		/// <summary>
		/// Reads the next 32-bit signed integer
		/// </summary>
		/// <returns></returns>
		public int NextInt()
		{
			var neg = false;
			int res = 0;
			SkipWhiteSpaces();
			if (!_check && CurrentChar() == '-')					// Check negative number
			{
				neg = true;
				_pos++;
			}
			while (!_check && !IsWhiteSpace(CurrentChar()))
			{
				var c = NextChar();
				if (c < '0' || c > '9') throw new ArgumentException("Character not unavailable");
				res = 10 * res + c - '0';
			}
			return neg ? -res : res;
		}
		/// <summary>
		/// Reads the next 64-bit signed integer (long)
		/// </summary>
		/// <returns></returns>
		public long NextLong()
		{
			var neg = false;
			long res = 0;
			SkipWhiteSpaces();
			if (!_check && CurrentChar() == '-')                    // Check negative number
			{
				neg = true;
				_pos++;
			}
			while (!_check && !IsWhiteSpace(CurrentChar()))
			{
				var c = NextChar();
				if (c < '0' || c > '9') throw new ArgumentException("Character not unavailable");
				res = 10 * res + c - '0';
			}
			return neg ? -res : res;
		}
		/// <summary>
		/// Reads the next line of characters
		/// </summary>
		/// <returns></returns>
		public string NextLine()
		{
			SkipUntilNextLine();
			if (_check) return "";
			var builder = new StringBuilder();
			while (!_check && !IsEndOfLine(CurrentChar()))
			{
				builder.Append(NextChar());
			}
			return builder.ToString();
		}
		/// <summary>
		/// Reads the next double-precision floating-point number
		/// </summary>
		/// <returns></returns>
		public double NextDouble()
		{
			SkipWhiteSpaces();
			var builder = new StringBuilder();
			while (!_check && !IsWhiteSpace(CurrentChar()))
			{
				builder.Append(NextChar());
			}
			return double.Parse(builder.ToString(), CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Reads the next 32-bit signed integer array with paramenter is size of array
		/// </summary>
		/// <returns></returns>
		public int[] NextIntArray(int size)
		{
			var res = new int[size];
			for (int i = 0; i < size; i++) res[i] = NextInt();
			return res;
		}
		/// <summary>
		/// Reads the next 64-bit signed integer (long) array with paramenter is size of array
		/// </summary>
		/// <returns></returns>
		public long[] NextLongArray(int size)
		{
			var res = new long[size];
			for (int i = 0; i < size; i++) res[i] = NextLong();
			return res;
		}
		/// <summary>
		/// Reads the next double array with paramenter is size of array
		/// </summary>
		/// <returns></returns>
		public double[] NextDoubleArray(int size)
		{
			var res = new double[size];
			for (int i = 0; i < size; i++) res[i] = NextDouble();
			return res;
		}
		/// <summary>
		/// Reads the next int matrix with paramenters are row number and column number
		/// </summary>
		/// <returns></returns>
		public int[][] NextIntMatrix(int row,int col)
		{
			var res = new int[row][];
			for (int i = 0; i < row; i++) res[i] = NextIntArray(col);
			return res;
		}
		/// <summary>
		/// Releases all resource used by TextReader object 
		/// </summary>
		/// <returns></returns>
		//public void Dispose()
		//{
		//	if (_closeReader) _reader.Close();
		//}
		public void Dispose()
		{
			((IDisposable)_reader).Dispose();
		}

		public static void Out(object obj) { _out.WriteLine(obj); }

	}

	public class TestRunTime
	{
		public delegate void RunTimeHandler();
		public static void Solustion(RunTimeHandler runTimeHandler)
		{
			var timer = new Stopwatch();
			timer.Start();
			runTimeHandler();
			timer.Stop();
			Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Solve problem in : "+ timer.ElapsedMilliseconds / 1000.0+ "s"));
		}
	}
}
