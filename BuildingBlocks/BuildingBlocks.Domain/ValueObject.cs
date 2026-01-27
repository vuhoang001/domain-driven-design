using System.Reflection;

namespace BuildingBlocks.Domain;

/// <summary>
/// Đoạn ValueObect triển khai so sánh giá trị cho các đối tượng DDD:
/// + Lưu trữ danh sách PropertyInfo và FieldInfo để tính toán một lần , bỏ qua các thành viên có IgnoreMemberAttribute.
/// + Equals so sánh kiểu và lần lượt so sánh với tất cả thuộc tính/trường, public/non-public theo giá trị.
/// + GetHashCode tạo hash từ tất cả thuộc tính và trường ( nhân với 23, cộng hash từng giá trị).
/// + Toán tử ==/!= dựa trên Equals, xử lý null an toàn.
/// + HashValue là hàm trợ giúp tính hash từng giá trị.
/// </summary>
public class ValueObject : IEquatable<ValueObject>
{
    private List<PropertyInfo>? _properties = null;

    private List<FieldInfo>? _fields = null;

    public bool Equals(ValueObject? other)
    {
        return Equals(other as object);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType()) return false;

        return GetProperties().All(p => PropertiesAreEqual(obj, p)) && GetFields().All(f => FieldsAreEqual(obj, f));
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            foreach (var prop in GetProperties())
            {
                var value = prop.GetValue(this, null);
                hash = HashValue(hash, value);
            }

            foreach (var field in GetFields())
            {
                var value = field.GetValue(this);
                hash = HashValue(hash, value);
            }

            return hash;
        }
    }

    private bool PropertiesAreEqual(object obj, PropertyInfo p)
    {
        return object.Equals(p.GetValue(this, null), p.GetValue(obj, null));
    }

    private bool FieldsAreEqual(object obj, FieldInfo f)
    {
        return object.Equals(f.GetValue(this), f.GetValue(obj));
    }


    /// <summary>
    /// Reflection chậm => Chỉ lấy danh sách một lần rồi cache lại.
    /// Loại bỏ các member có IgnoreMemberAttribute để không so sánh chúng. 
    /// </summary>
    /// <returns></returns>
    private IEnumerable<PropertyInfo> GetProperties()
    {
        if (this._properties == null)
        {
            this._properties = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                .ToList();

            // Not available in Core
            // !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute))).ToList();
        }

        return this._properties;
    }

    private IEnumerable<FieldInfo> GetFields()
    {
        if (this._fields == null)
        {
            this._fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                .ToList();
        }

        return this._fields;
    }

    public int HashValue(int seed, object? value)
    {
        var currentHash = value != null ? value.GetHashCode() : 0;
        return (seed * 23) + currentHash;
    }

    public static bool operator ==(ValueObject obj1, ValueObject obj2)
    {
        if (object.Equals(obj1, null))
        {
            if (object.Equals(obj2, null))
            {
                return true;
            }

            return false;
        }

        return obj1.Equals(obj2);
    }

    public static bool operator !=(ValueObject obj1, ValueObject obj2)
    {
        return !(obj1 == obj2);
    }
}