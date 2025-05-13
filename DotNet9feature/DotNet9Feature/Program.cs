// See https://aka.ms/new-console-template for more information



using DotNet9Feature;

Console.WriteLine("Hello, World!");
// if (Feature.IsFeatureEnabled) {
//     Feature.DoSomething();
// }



List<(long CourseId, long LessonDurationSeconds)> totalTime = new();

var completedLesson = new List<CompletedLesson>
 {
    new (1,1,434),
    new (1,2,524),
    new (2,1,34),
    new (2,1,1034),
    new (3,1,11134),
    new (4,1,451121)
  };

foreach (var item in completedLesson) {
    totalTime.Add((item.CourseId,item.LessonDurationSeconds));
}

var totalSeenTime = totalTime.AggregateBy(m => m.CourseId, _ => 0m, (seconds, item) => decimal.Add(seconds, item.LessonDurationSeconds));

foreach(var item in totalSeenTime) {
    Console.WriteLine($"Course Id : {item.Key} : {item.Value} seconds");
}

record CompletedLesson(long CourseId,long LessonId, long LessonDurationSeconds);
