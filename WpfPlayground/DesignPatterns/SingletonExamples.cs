using System;

namespace WpfPlayground.DesignPatterns
{
    public class SingletonExamples
    {        
        public SingletonExamples()
        {                       
        }
    }

    public class Singleton
    {
        private static Singleton instance;

        private Singleton() { }

        public static Singleton GetInstance()
        {
            if (instance == null) { instance = new Singleton(); }
            return instance;
        }

        //Getter version
        //public static Singleton Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new Singleton();
        //        }
        //        return instance;
        //    }
        //}
    }

    /*The main disadvantage of this implementation, however, is that it is not safe for multithreaded environments. 
  * If separate threads of execution enter the Instance property method at the same time, more that one instance 
  * of the Singleton object may be created. Each thread could execute the following statement and decide that a 
  * new instance has to be created:
  * 
  * if (instance == null)
  * 
  * Various approaches solve this problem. One approach is to use an idiom referred to as Double-Check Locking
  * However, C# in combination with the common language runtime provides a static initialization approach, 
  * which circumvents these issues without requiring the developer to explicitly code for thread safety
  * 
  * Static Initialization
  * One of the reasons Design Patterns [Gamma95] avoided static initialization is because the C++ specification 
  * left some ambiguity around the initialization order of static variables. Fortunately, the .NET Framework 
  * resolves this ambiguity through its handling of variable initialization:  * 
  */

    public sealed class Singleton2
    {
        private static readonly Singleton2 instance = new Singleton2();

        private Singleton2() { }        

        public static Singleton2 Instance
        {
            get
            {
                return instance;
            }
        }
    }

    /*
     * In this strategy, the instance is created the first time any member of the class is referenced. 
     * The common language runtime takes care of the variable initialization. The class is marked sealed to 
     * prevent derivation, which could add instances. For a discussion of the pros and cons of marking a class 
     * sealed, see [Sells03]. In addition, the variable is marked readonly, which means that it can be assigned 
     * only during static initialization (which is shown here) or in a class constructor.
     * 
     * This implementation is similar to the preceding example, except that it relies on the common language runtime 
     * to initialize the variable. It still addresses the two basic problems that the Singleton pattern is trying to 
     * solve: global access and instantiation control. The public static property provides a global access point to 
     * the instance. Also, because the constructor is private, the Singleton class cannot be instantiated outside of 
     * the class itself; therefore, the variable refers to the only instance that can exist in the system.
     * 
     * Because the Singleton instance is referenced by a private static member variable, the instantiation does not 
     * occur until the class is first referenced by a call to the Instance property. This solution therefore implements 
     * a form of the lazy instantiation property, as in the Design Patterns form of Singleton.
     * 
     * The only potential downside of this approach is that you have less control over the mechanics of the instantiation. 
     * In the Design Patterns form, you were able to use a nondefault constructor or perform other tasks before 
     * the instantiation. Because the .NET Framework performs the initialization in this solution, you do not have 
     * these options. In most cases, static initialization is the preferred approach for implementing a Singleton in .NET.     * 
     */


    /* Multithreaded Singleton
     * Static initialization is suitable for most situations. When your application must delay the instantiation, use a 
     * non-default constructor or perform other tasks before the instantiation, and work in a multithreaded environment, 
     * you need a different solution. Cases do exist, however, in which you cannot rely on the common language runtime 
     * to ensure thread safety, as in the Static Initialization example. In such cases, you must use specific language 
     * capabilities to ensure that only one instance of the object is created in the presence of multiple threads. One 
     * of the more common solutions is to use the Double-Check Locking [Lea99] idiom to keep separate threads from 
     * creating new instances of the singleton at the same time.
     * 
     * The following implementation allows only a single thread to enter the critical area, which the lock block 
     * identifies, when no instance of Singleton has yet been created:     
     */

    public sealed class Singleton3
    {
        private static volatile Singleton3 instance;
        private static object syncRoot = new Object();

        private Singleton3() { }

        public static Singleton3 Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Singleton3();
                    }
                }

                return instance;
            }
        }
    }
}