namespace Infrastructure.Shared;

public class Utils
{
    public static void CopyObjectToAnother(object originObject, object destinyObject)
    {
        var entityType = originObject.GetType();
        var properties = entityType.GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(originObject);
            if (value != null)
            {
                var destinyProperty = destinyObject.GetType().GetProperty(property.Name);
                if (destinyProperty != null)
                {
                    destinyProperty.SetValue(destinyObject, value);
                }
            }
        }
    }
}
