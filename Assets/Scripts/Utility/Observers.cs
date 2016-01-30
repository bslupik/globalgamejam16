using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//


/// <summary>
/// Implementation of the Observer Design Pattern.
/// </summary>
/// <remarks>
/// You can either use the generic Observable&lt;T&gt; class and do things small-scale, or you can have your script implement IObserver&lt;Message&gt;>, which has automatic linking via singleton.
/// To use the singleton:
///   -Have your monobehaviour implement IObserver&lt;tMessage&gt;, which requires you to implement a public void Notify(Message message) {}, in which you handle the incoming message.
///   -Call the line &quot;Observers.Subscribe(this, params string[] messageTypes)&quot;. 
///           For example, the line of code to listen to messages tagged &quot;AttackMessage&quot; and &quot;OptionsMessage&quot; would be Observers.Subscribe(this, &quot;AttackMessage&quot;, &quot;OptionsMessage&quot;);
///   -If the script is ever deactivated or destroyed, call &quot;Observers.Unsubscribe(this, params string[] messageTypes)&quot;. Same formatting as above
/// To send a message:
///   -Call Observers.Post(new Message([...]));
///           For example, to send a basic message tagged &quot;AttackMessage&quot;, the code would be Observers.Post(new Message(&quot;AttackMessage&quot;));
/// Note: I HEAVILY suggest you use the Tags class to set message types in code, instead of using string literals as in these examples.
/// I also advise that sub-classes of message (to carry data) should be written in a different file. It'll make it easier for you to update this file in case of changes.
/// </remarks>
/// <typeparam name="T">The type of the message passed</typeparam>
public class Observable<T>
{
    List<IObserver<T>> observers;

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public Observable()
    {
        observers = new List<IObserver<T>>();
    }

    /// <summary>
    /// Sets the <see cref="IObserver"/> to be notifed.
    /// </summary>
    /// <param name="subscriber">The <see cref="IObserver"/> to be notifed.</param>
    public void Subscribe(IObserver<T> subscriber) { observers.Add(subscriber); }

    /// <summary>
    /// Removes the <see cref="IObserver"/> from notifications.
    /// </summary>
    /// <param name="subscriber">The <see cref="IObserver"/> to remove.</param>
    public void Unsubscribe(IObserver<T> subscriber) { observers.Remove(subscriber); }

    /// <summary>
    /// Notifies all subscribed <see cref="IObserver"/>s with the message.
    /// </summary>
    /// <param name="message">The object to be sent as a parameter to all subscribers.</param>
    public void Post(T message)
    {
        for (int i = observers.Count - 1; i >= 0; i--) //backwards iteration to allow elements to be removed
        {
            observers[i].Notify(message);
        }
    }

    /// <summary>
    /// Removes all subscribers.
    /// </summary>
    public void Clear()
    {
        observers.Clear();
    }
}

/// <summary>
/// Represents a class that can subscribe to an <see cref="IObservable"/>.
/// </summary>
/// <typeparam name="T">The type of the message passed</typeparam>
public interface IObserver<T>
{
    /// <summary>
    /// Called by the <see cref="IObservable"/>s that this object is subscribed to when an event happens.
    /// </summary>
    /// <param name="message">The message sent</param>
    void Notify(T message);
}

//interface to indicate that a monobehaviour has an observable on it
/// <summary>
/// Represents a class that wraps an <see cref="Observable"/>.
/// </summary>
/// <typeparam name="T">The type of message passed by the wrapped <see cref="Observable"/></typeparam>
public interface IObservable<T>
{
    /// <summary>
    /// Returns the wrapped <see cref="Observable"/> of the class.
    /// </summary>
    /// <param name="self">A reference to the instance the method is being calld on, so that there can be method overloading with generics.</param>
    /// <returns>The wrapped <see cref="Observable"/></returns>
    Observable<T> Observable(IObservable<T> self); // self isJustHereForFunctionOverloading
    //  { return <*your observable here*>; }
}
/// <summary>
/// A set of extension methods for <see cref="IObservable"/>,
/// </summary>
public static class IObservableExtension
{
    /// <summary>
    /// Removes the need to pass in the <see cref="IObservable"/> as a parameter in everyday use.
    /// </summary>
    /// <remarks>
    /// <typeparam name="T">The type of message passed by the wrapped <see cref="Observable"/></typeparam>
    /// <param name="self">The <see cref="IObservable"/> the method is called on</param>
    /// <returns>The wrapped <see cref="Observable"/></returns>
    public static Observable<T> Observable<T>(this IObservable<T> self)
    {
        return self.Observable(self);
    }

    /// <summary>
    /// Calls the wrapped <see cref="Observable"/>'s <see cref="Subscribe"> method.
    /// </summary>
    /// <typeparam name="T">The type of message passed by the wrapped <see cref="Observable"/></typeparam>
    /// <param name="self">The <see cref="IObservable"/> the method is called on</param>
    /// <param name="subscriber">The <see cref="IObserver"/> to be notifed.</param>
    public static void Subscribe<T>(this IObservable<T> self, IObserver<T> subscriber)
    {
        self.Observable().Subscribe(subscriber);
    }

    /// <summary>
    /// Calls the wrapped <see cref="Observable"/>'s <see cref="Unsubscribe"> method.
    /// </summary>
    /// <typeparam name="T">The type of message passed by the wrapped <see cref="Observable"/></typeparam>
    /// <param name="self">The <see cref="IObservable"/> the method is called on</param>
    /// <param name="subscriber">The <see cref="IObserver"/> to remove.</param>
    public static void Unsubscribe<T>(this IObservable<T> self, IObserver<T> subscriber)
    {
        self.Observable().Unsubscribe(subscriber);
    }

    /// <summary>
    /// Calls the wrapped <see cref="Observable"/>'s <see cref="Post"> method.
    /// </summary>
    /// <typeparam name="T">The type of message passed by the wrapped <see cref="Observable"/></typeparam>
    /// <param name="self">The <see cref="IObservable"/> the method is called on</param>
    /// <param name="message">The object to be sent as a parameter to all subscribers.</param>
    public static void Post<T>(this IObservable<T> self, T message)
    {
        self.Post(message);
    }
}

//base message; child classes can be used to carry data
/// <summary>
/// Base Message class used in <see cref="Observers"/>.
/// </summary>
public class Message 
{
    /// <summary>
    /// The type of the message as a <see cref="String"/>, used by <see cref="Observers"/> for routing.
    /// </summary>
    public readonly string messageType;
    /// <summary>
    /// Base constructor
    /// </summary>
    /// <param name="type">The type of the message as a <see cref="String"/>.</param>
    public Message(string type)
    {
        messageType = type;
    }
}
//I suggest that child classes of message have hard-coded names to make filtering and casting the incoming messages easier (you can use switch statements that way)

//static class that uses strings to route messages. messages must be cast on the recieving end
/// <summary>
/// A global messaging system using <see cref="Observables"/>.
/// </summary>
public static class Observers
{
    static Dictionary<string, Observable<Message>> theObservables = new Dictionary<string, Observable<Message>>();

    /// <summary>
    /// Sets the <see cref="IObserver"/> to be notified.
    /// </summary>
    /// <param name="subscriber">The <see cref="IObserver"/> to be notified.</param>
    /// <param name="messageTypes">The list of <see cref="messageType"/>s the subscriber wants to subscribe to.</param>
    public static void Subscribe(IObserver<Message> subscriber, params string[] messageTypes)
    {
        foreach (string messageType in messageTypes)
        {
            if (!theObservables.ContainsKey(messageType))
                theObservables[messageType] = new Observable<Message>();

            theObservables[messageType].Subscribe(subscriber);
        }
    }

    /// <summary>
    /// Removes the <see cref="IObserver"/> from being notified.
    /// </summary>
    /// <param name="subscriber">The <see cref="IObserver"/> to remove.</param>
    /// <param name="messageTypes">The list of <see cref="messageType"/>s the subscriber wants to be removed from.</param>
    public static void Unsubscribe(IObserver<Message> subscriber, params string[] messageTypes)
    {
        foreach (string messageType in messageTypes)
            if (theObservables.ContainsKey(messageType))
                theObservables[messageType].Unsubscribe(subscriber);
    }

    /// <summary>
    /// Notifies all subscribed <see cref="IObserver"/>s with the message.
    /// </summary>
    /// <param name="message">The <see cref="Message"/> to be sent as a parameter to all subscribers.</param>
    public static void Post(Message message)
    {
        if(theObservables.ContainsKey(message.messageType))
            theObservables[message.messageType].Post(message);
    }


    /// <summary>
    /// Unsubscribes all subscribers.
    /// </summary>
    /// <param name="messageTypes">The list of <see cref="messageType"/>s to clear.</param>
    public static void Clear(params string[] messageTypes)
    {
        foreach (string messageType in messageTypes)
            if (theObservables.ContainsKey(messageType))
                theObservables[messageType].Clear();
    }
}