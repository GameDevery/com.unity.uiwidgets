﻿using System;
using UIWidgets.foundation;
using UIWidgets.ui;

namespace UIWidgets.service
{
    public class TextRange: IEquatable<TextRange>
    {
        public readonly int start;
        public readonly int end;

        public static TextRange collapsed(int offset)
        {
            D.assert(offset >= -1);
            return new TextRange(offset, offset);
        }
        
        public static readonly TextRange empty = new TextRange(-1, -1);
        
        public TextRange(int start, int end)
        {
            D.assert(start >= -1);
            D.assert(end >= -1);
            this.start = start;
            this.end = end;
        }

        public bool isValid
        {
            get { return start >= 0 && end >= 0; }
        }
        
        public bool isCollapsed
        {
            get { return start == end; }
        }

        public bool isNormalized
        {
            get { return start <= end; }
        }

        public string textBefore(string text)
        {
            D.assert(isNormalized);
            return text.Substring(0, start);
        }
        
        public string textAfter(string text)
        {
            D.assert(isNormalized);
            return text.Substring(end);
        }

        public string textInside(string text)
        {
            D.assert(isNormalized);
            return text.Substring(start, end - start);
        }

        public bool Equals(TextRange other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return start == other.start && end == other.end;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TextRange) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (start * 397) ^ end;
            }
        }

        public static bool operator ==(TextRange left, TextRange right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TextRange left, TextRange right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("TextRange Start: {0}, End: {1}", start, end);
        }
    }

    public class TextSelection : TextRange, IEquatable<TextSelection>
    {
        public readonly int baseOffset;
        public readonly int extendOffset;
        public readonly TextAffinity affinity;
        public readonly bool isDirectional;

        public TextSelection(int baseOffset, int extendOffset, TextAffinity affinity = TextAffinity.downstream,
            bool isDirectional = false):base(baseOffset < extendOffset ? baseOffset: extendOffset, 
            baseOffset < extendOffset ? extendOffset : baseOffset )
        {
            this.baseOffset = baseOffset;
            this.extendOffset = extendOffset;
            this.affinity = affinity;
            this.isDirectional = isDirectional;
        }

        public static TextSelection collapsed(int offset, TextAffinity affinity = TextAffinity.downstream)
        {
            return new TextSelection(offset, offset, affinity, false);
        }

        public static TextSelection fromPosition(TextPosition position)
        {
            return collapsed(position.offset, position.affinity);
        }
        
        public TextPosition basePos
        {
            get
            {
                return new TextPosition(offset: baseOffset, affinity: affinity);
            }
        }

        public TextPosition extendPos
        {
            get
            {
                return new TextPosition(offset: extendOffset, affinity: affinity);
            }
        }

        public bool Equals(TextSelection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return baseOffset == other.baseOffset && extendOffset == other.extendOffset && 
                   affinity == other.affinity && isDirectional == other.isDirectional;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TextSelection) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 0;
                hashCode = (hashCode * 397) ^ baseOffset;
                hashCode = (hashCode * 397) ^ extendOffset;
                hashCode = (hashCode * 397) ^ (int) affinity;
                hashCode = (hashCode * 397) ^ isDirectional.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(TextSelection left, TextSelection right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TextSelection left, TextSelection right)
        {
            return !Equals(left, right);
        }

        public TextSelection copyWith(int? baseOffset = null, int? extendOffset = null, TextAffinity? affinity = null,
            bool? isDirectional = null)
        {
            return new TextSelection(
                baseOffset??this.baseOffset, extendOffset??this.extendOffset, affinity??this.affinity,
                isDirectional??this.isDirectional
                );
        }
    }
}