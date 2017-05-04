using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAUtils_Test {
    class ExampleSeqUsage {
        // public static void NonBlocking_event_driven() {
        //     IObservable<object> ob = Observable.Create<object>(
        //         observer => {
        //             var timer = new System.Timers.Timer();
        //             timer.Interval = 1000;
        //             timer.Elapsed += (s, e) => observer.OnNext("Something");
        //             observer.OnNext(new List<int>{5});
        //             timer.Start();
        //             return Disposable.Empty;
        //         }
        //     );

        //     var subject = new Subject<object>();
        //     ob.Subscribe(subject);
        //     subject.Where(o_ => (o_ as string) != null)
        //            .Take(5)
        //            .Aggregate((obj1, obj2) => (string)obj1 + " " + (string)obj2)
        //            .Subscribe(Console.WriteLine);

        //     Console.ReadLine();
        //     subject.Dispose();
        // }
    }
}
