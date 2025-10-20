using System.Linq.Expressions;
using System.Windows.Forms;

namespace LastMinuteTours.Infrostructure
{
    public static class Extensions
    {
        public static void AddBinding<TControl, TSource>(this TControl control, 
            Expression<Func<TControl, object>> destinationProperty,
            TSource source,
            Expression<Func<TSource, object>> sourceProperty)
            where TControl : Control
            where TSource : class
        {

        }
    }
}
