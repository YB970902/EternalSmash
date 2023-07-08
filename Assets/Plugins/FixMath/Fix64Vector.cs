using UnityEngine;

namespace FixMath.NET
{
    public struct FixVector3
    {
        public Fix64 x { get; private set; }
        public Fix64 y { get; private set; }
        public Fix64 z { get; private set; }

        /// <summary> 벡터의 크기. 제곱근 연산까지 되어 정확하다. </summary>
        private Fix64 mag;
        /// <summary> 벡터의 크기. 제곱근 연산이 되지 않아 정확하지 않다. </summary>
        private Fix64 sqrtMag;

        public static readonly FixVector3 Zero = new FixVector3(0, 0, 0);
        public static readonly FixVector3 Forward = new FixVector3(0, 0, 1);
        public static readonly FixVector3 Back = new FixVector3(0, 0, -1);
        public static readonly FixVector3 Left = new FixVector3(-1, 0, 0);
        public static readonly FixVector3 Right = new FixVector3(1, 0, 0);
        public static readonly FixVector3 Up = new FixVector3(0, 1, 0);
        public static readonly FixVector3 Down = new FixVector3(0, -1, 0);

        public FixVector3(Fix64 _x, Fix64 _y, Fix64 _z)
        {
            x = _x;
            y = _y;
            z = _z;

            mag = Fix64.Zero;
            sqrtMag = Fix64.Zero;
        }

        public FixVector3(int _x, int _y, int _z)
        {
            x = (Fix64)_x;
            y = (Fix64)_y;
            z = (Fix64)_z;

            mag = Fix64.Zero;
            sqrtMag = Fix64.Zero;
        }

        public static FixVector3 operator + (FixVector3 _lhs, FixVector3 _rhs)
        {
            return new FixVector3(_lhs.x + _rhs.x, _lhs.y + _rhs.y, _lhs.z + _rhs.z);
        }

        public static FixVector3 operator - (FixVector3 _lhs, FixVector3 _rhs)
        {
            return new FixVector3(_lhs.x - _rhs.x, _lhs.y - _rhs.y, _lhs.z - _rhs.z);
        }

        public static FixVector3 operator * (FixVector3 _vec, Fix64 _val)
        {
            return new FixVector3(_vec.x * _val, _vec.y * _val, _vec.z * _val);
        }

        public Fix64 Magnitude()
        {
            if(sqrtMag <= Fix64.Zero)
            {
                sqrtMag = Fix64.Pow2(x) + Fix64.Pow2(y) + Fix64.Pow2(z);
                mag = Fix64.Sqrt(sqrtMag);
            }
            else if(mag <= Fix64.Zero)
            {
                mag = Fix64.Sqrt(sqrtMag);
            }
            
            return mag;
        }

        public Fix64 SqrtMagnitude()
        {
            if (sqrtMag <= Fix64.Zero)
            {
                sqrtMag = Fix64.Pow2(x) + Fix64.Pow2(y) + Fix64.Pow2(z);
            }

            return sqrtMag;
        }

        public static Fix64 GetSqrtDisatnce(in FixVector3 _lhs, in FixVector3 _rhs)
        {
            return Fix64.Pow2(_lhs.x - _rhs.x) + Fix64.Pow2(_lhs.y - _rhs.y) + Fix64.Pow2(_lhs.z - _rhs.z);
        }

        public static Fix64 GetDistance(in FixVector3 _lhs, in FixVector3 _rhs)
        {
            return Fix64.Sqrt(GetSqrtDisatnce(_lhs, _rhs));
        }

        public static Fix64 Dot(in FixVector3 _lhs, in FixVector3 _rhs)
        {
            return _lhs.x * _rhs.x + _lhs.y * _rhs.y + _lhs.z * _rhs.z;
        }

        public static FixVector3 Cross(in FixVector3 _lhs, in FixVector3 _rhs)
        {
            return new FixVector3(
                _lhs.y * _rhs.z - _lhs.z * _rhs.y,
                _lhs.z * _rhs.x - _lhs.x * _rhs.z,
                _lhs.x * _rhs.y - _lhs.y * _rhs.x);
        }

        public static implicit operator Vector3(FixVector3 _vec)
        {
            return new Vector3((float)_vec.x, (float)_vec.y, (float)_vec.z);
        }
    }
    
    public struct FixVector2
    {
        public Fix64 x { get; private set; }
        public Fix64 y { get; private set; }

        /// <summary> 벡터의 크기. 제곱근 연산까지 되어 정확하다. </summary>
        private Fix64 mag;
        /// <summary> 벡터의 크기. 제곱근 연산이 되지 않아 정확하지 않다. </summary>
        private Fix64 sqrtMag;

        public static readonly FixVector2 Zero = new FixVector2(0, 0);
        public static readonly FixVector2 Left = new FixVector2(-1, 0);
        public static readonly FixVector2 Right = new FixVector2(1, 0);
        public static readonly FixVector2 Up = new FixVector2(0, 1);
        public static readonly FixVector2 Down = new FixVector2(0, -1);

        public FixVector2(Fix64 _x, Fix64 _y)
        {
            x = _x;
            y = _y;

            mag = Fix64.Zero;
            sqrtMag = Fix64.Zero;
        }

        public FixVector2(int _x, int _y)
        {
            x = (Fix64)_x;
            y = (Fix64)_y;

            mag = Fix64.Zero;
            sqrtMag = Fix64.Zero;
        }
        
        public FixVector2(float _x, float _y)
        {
            x = (Fix64)_x;
            y = (Fix64)_y;

            mag = Fix64.Zero;
            sqrtMag = Fix64.Zero;
        }

        public static FixVector2 operator + (FixVector2 _lhs, FixVector2 _rhs)
        {
            return new FixVector2(_lhs.x + _rhs.x, _lhs.y + _rhs.y);
        }

        public static FixVector2 operator - (FixVector2 _lhs, FixVector2 _rhs)
        {
            return new FixVector2(_lhs.x - _rhs.x, _lhs.y - _rhs.y);
        }

        public static FixVector2 operator * (FixVector2 _vec, Fix64 _val)
        {
            return new FixVector2(_vec.x * _val, _vec.y * _val);
        }

        public Fix64 Magnitude()
        {
            if(sqrtMag <= Fix64.Zero)
            {
                sqrtMag = Fix64.Pow2(x) + Fix64.Pow2(y);
                mag = Fix64.Sqrt(sqrtMag);
            }
            else if(mag <= Fix64.Zero)
            {
                mag = Fix64.Sqrt(sqrtMag);
            }
            
            return mag;
        }

        public Fix64 SqrtMagnitude()
        {
            if (sqrtMag <= Fix64.Zero)
            {
                sqrtMag = Fix64.Pow2(x) + Fix64.Pow2(y);
            }

            return sqrtMag;
        }

        public static Fix64 GetSqrtDisatnce(in FixVector2 _lhs, in FixVector2 _rhs)
        {
            return Fix64.Pow2(_lhs.x - _rhs.x) + Fix64.Pow2(_lhs.y - _rhs.y);
        }

        public static Fix64 GetDistance(in FixVector2 _lhs, in FixVector2 _rhs)
        {
            return Fix64.Sqrt(GetSqrtDisatnce(_lhs, _rhs));
        }

        public static Fix64 Dot(in FixVector2 _lhs, in FixVector2 _rhs)
        {
            return _lhs.x * _rhs.x + _lhs.y * _rhs.y;
        }

        public static implicit operator Vector2(FixVector2 _vec)
        {
            return new Vector3((float)_vec.x, (float)_vec.y);
        }

        public static FixVector2 Lerp(FixVector2 a, FixVector2 b, Fix64 t)
        {
            return (b - a) * t + a;
        }
    }
}