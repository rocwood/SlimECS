using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SlimECS
{
	public class SortedList<TKey, TValue>
	{
		private TKey[] keys;
		private TValue[] values;
		private int _size;
		private int version;
		private IComparer<TKey> comparer;
		private ValueList valueList;

		static TKey[] emptyKeys = new TKey[0];
		static TValue[] emptyValues = new TValue[0];

		private const int _defaultCapacity = 4;

		public SortedList()
		{
			keys = emptyKeys;
			values = emptyValues;
			_size = 0;
			comparer = Comparer<TKey>.Default;
		}

		public SortedList(int capacity)
		{
			//if (capacity < 0)
			//	ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNumRequired);
			keys = new TKey[capacity];
			values = new TValue[capacity];
			comparer = Comparer<TKey>.Default;
		}

		public void Add(TKey key, TValue value)
		{
			//if (key == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			int i = Array.BinarySearch<TKey>(keys, 0, _size, key, comparer);
			if (i >= 0)
				return;
			//	ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
			Insert(~i, key, value);
		}

		public int Capacity
		{
			get {
				return keys.Length;
			}
			set {
				if (value != keys.Length)
				{
					if (value < _size)
					{
						return;
						//ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
					}

					if (value > 0)
					{
						TKey[] newKeys = new TKey[value];
						TValue[] newValues = new TValue[value];
						if (_size > 0)
						{
							Array.Copy(keys, 0, newKeys, 0, _size);
							Array.Copy(values, 0, newValues, 0, _size);
						}
						keys = newKeys;
						values = newValues;
					}
					else
					{
						keys = emptyKeys;
						values = emptyValues;
					}
				}
			}
		}

		public IComparer<TKey> Comparer
		{
			get {
				return comparer;
			}
		}

		public int Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				return _size;
			}
		}

		public IReadOnlyList<TValue> Values
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				return GetValueListHelper();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TValue GetAt(int index)
		{
			return values[index];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ValueList GetValueListHelper()
		{
			if (valueList == null)
				valueList = new ValueList(this);
			return valueList;
		}

		public void Clear()
		{
			// clear does not change the capacity
			version++;
			// Don't need to doc this but we clear the elements so that the gc can reclaim the references.
			Array.Clear(keys, 0, _size);
			Array.Clear(values, 0, _size);
			_size = 0;
		}

		public bool ContainsKey(TKey key)
		{
			return IndexOfKey(key) >= 0;
		}

		public bool ContainsValue(TValue value)
		{
			return IndexOfValue(value) >= 0;
		}


		private const int MaxArrayLength = 0X7FEFFFFF;

		private void EnsureCapacity(int min)
		{
			int newCapacity = keys.Length == 0 ? _defaultCapacity : keys.Length * 2;
			// Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
			// Note that this check works even when _items.Length overflowed thanks to the (uint) cast
			if ((uint)newCapacity > MaxArrayLength) newCapacity = MaxArrayLength;
			if (newCapacity < min) newCapacity = min;
			Capacity = newCapacity;
		}

		// Returns the value of the entry at the given index.
		// 
		private TValue GetByIndex(int index)
		{
			//if (index < 0 || index >= _size)
			//	ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			return values[index];
		}

		public TValue this[TKey key]
		{
			get {
				int i = IndexOfKey(key);
				if (i >= 0)
					return values[i];

				//ThrowHelper.ThrowKeyNotFoundException();
				return default(TValue);
			}
			set {
				//if (((Object)key) == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
				int i = Array.BinarySearch<TKey>(keys, 0, _size, key, comparer);
				if (i >= 0)
				{
					values[i] = value;
					version++;
					return;
				}
				Insert(~i, key, value);
			}
		}

		public int IndexOfKey(TKey key)
		{
			//if (key == null)
			//	ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			int ret = Array.BinarySearch<TKey>(keys, 0, _size, key, comparer);
			return ret >= 0 ? ret : -1;
		}

		public int IndexOfValue(TValue value)
		{
			return Array.IndexOf(values, value, 0, _size);
		}

		private void Insert(int index, TKey key, TValue value)
		{
			if (_size == keys.Length) EnsureCapacity(_size + 1);
			if (index < _size)
			{
				Array.Copy(keys, index, keys, index + 1, _size - index);
				Array.Copy(values, index, values, index + 1, _size - index);
			}
			keys[index] = key;
			values[index] = value;
			_size++;
			version++;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			int i = IndexOfKey(key);
			if (i >= 0)
			{
				value = values[i];
				return true;
			}

			value = default(TValue);
			return false;
		}

		public void RemoveAt(int index)
		{
			//if (index < 0 || index >= _size) ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			_size--;
			if (index < _size)
			{
				Array.Copy(keys, index + 1, keys, index, _size - index);
				Array.Copy(values, index + 1, values, index, _size - index);
			}
			keys[_size] = default(TKey);
			values[_size] = default(TValue);
			version++;
		}

		public bool Remove(TKey key)
		{
			int i = IndexOfKey(key);
			if (i >= 0)
				RemoveAt(i);
			return i >= 0;
		}

		public void TrimExcess()
		{
			int threshold = (int)(((double)keys.Length) * 0.9);
			if (_size < threshold)
			{
				Capacity = _size;
			}
		}

		private struct SortedListValueEnumerator : IEnumerator<TValue>, IEnumerator
		{
			private SortedList<TKey, TValue> _sortedList;
			private int index;
			private int version;
			//private TValue currentValue;

			internal SortedListValueEnumerator(SortedList<TKey, TValue> sortedList)
			{
				_sortedList = sortedList;

				version = sortedList.version;
				index = -1;
			}

			public void Dispose()
			{
				index = -1;
				//currentValue = default(TValue);
			}

			public bool MoveNext()
			{
				if (version != _sortedList.version)
				{
					//ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}

				/*
				if ((uint)index < (uint)_sortedList.Count)
				{
					//currentValue = _sortedList.values[index];
					index++;
					return true;
				}

				index = _sortedList.Count + 1;
				currentValue = default(TValue);
				return false;
				*/

				return ++index < _sortedList.Count;
			}

			public TValue Current
			{
				get {
					return _sortedList.values[index];
				}
			}

			object IEnumerator.Current
			{
				get {
					//if (index == 0 || (index == _sortedList.Count + 1))
					//{
					//	//ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					//}

					//return currentValue;

					return _sortedList.values[index];
				}
			}

			void IEnumerator.Reset()
			{
				if (version != _sortedList.version)
				{
					//ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				index = -1;
				//currentValue = default(TValue);
			}
		}

		private sealed class ValueList : IReadOnlyList<TValue>
		{
			private SortedList<TKey, TValue> _dict;

			internal ValueList(SortedList<TKey, TValue> dictionary)
			{
				this._dict = dictionary;
			}

			public int Count
			{
				get { return _dict._size; }
			}

			public TValue this[int index]
			{
				get {
					return _dict.GetByIndex(index);
				}
			}

			public IEnumerator<TValue> GetEnumerator()
			{
				return new SortedListValueEnumerator(_dict);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SortedListValueEnumerator(_dict);
			}
		}
	}
}
