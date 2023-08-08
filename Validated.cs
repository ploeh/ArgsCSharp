namespace Ploeh.Katas.ArgsCSharp;

public sealed class Validated<F, S>
{
    private interface IValidation
    {
        T Match<T>(Func<F, T> onFailure, Func<S, T> onSuccess);
    }

    private readonly IValidation imp;

    private Validated(IValidation imp)
    {
        this.imp = imp;
    }

    internal static Validated<F, S> Succeed(S success)
    {
        return new Validated<F, S>(new Success(success));
    }

    internal static Validated<F, S> Fail(F failure)
    {
        return new Validated<F, S>(new Failure(failure));
    }

    public T Match<T>(Func<F, T> onFailure, Func<S, T> onSuccess)
    {
        return imp.Match(onFailure, onSuccess);
    }

    public Validated<F1, S1> SelectBoth<F1, S1>(
        Func<F, F1> selectFailure,
        Func<S, S1> selectSuccess)
    {
        return Match(
            f => Validated.Fail<F1, S1>(selectFailure(f)),
            s => Validated.Succeed<F1, S1>(selectSuccess(s)));
    }

    public Validated<F1, S> SelectFailure<F1>(
        Func<F, F1> selectFailure)
    {
        return SelectBoth(selectFailure, s => s);
    }

    public Validated<F, S1> SelectSuccess<S1>(
        Func<S, S1> selectSuccess)
    {
        return SelectBoth(f => f, selectSuccess);
    }

    public Validated<F, S1> Select<S1>(
        Func<S, S1> selector)
    {
        return SelectSuccess(selector);
    }

    public override bool Equals(object? obj)
    {
        return obj is Validated<F, S> validated &&
               EqualityComparer<Validated<F, S>.IValidation>.Default.Equals(imp, validated.imp);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(imp);
    }

    private sealed class Success : IValidation
    {
        private readonly S success;

        public Success(S success)
        {
            this.success = success;
        }

        public override bool Equals(object? obj)
        {
            return obj is Validated<F, S>.Success success &&
                   EqualityComparer<S>.Default.Equals(this.success, success.success);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(success);
        }

        public T Match<T>(
            Func<F, T> onFailure,
            Func<S, T> onSuccess)
        {
            return onSuccess(success);
        }
    }

    private sealed class Failure : IValidation
    {
        private readonly F failure;

        public Failure(F failure)
        {
            this.failure = failure;
        }

        public override bool Equals(object? obj)
        {
            return obj is Validated<F, S>.Failure failure &&
                   EqualityComparer<F>.Default.Equals(this.failure, failure.failure);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(failure);
        }

        public T Match<T>(
            Func<F, T> onFailure,
            Func<S, T> onSuccess)
        {
            return onFailure(failure);
        }
    }
}

public static class Validated
{
    public static Validated<F, S> Succeed<F, S>(
        S success)
    {
        return Validated<F, S>.Succeed(success);
    }

    public static Validated<F, S> Fail<F, S>(
        F failure)
    {
        return Validated<F, S>.Fail(failure);
    }

    public static Validated<F, S> Apply<F, T, S>(
        this Validated<F, Func<T, S>> selector,
        Validated<F, T> source,
        Func<F, F, F> combine)
    {
        if (selector is null)
            throw new ArgumentNullException(nameof(selector));

        return selector.Match(
            f1 => source.Match(
                f2 => Fail<F, S>(combine(f1, f2)),
                _ => Fail<F, S>(f1)),
            map => source.Match(
                f2 => Fail<F, S>(f2),
                x => Succeed<F, S>(map(x))));
    }

    public static Validated<F, Func<T2, S>> Apply<F, T1, T2, S>(
        this Validated<F, Func<T1, T2, S>> selector,
        Validated<F, T1> source,
        Func<F, F, F> combine)
    {
        if (selector is null)
            throw new ArgumentNullException(nameof(selector));

        return selector.Match(
            f1 => source.Match(
                f2 => Fail<F, Func<T2, S>>(combine(f1, f2)),
                _ => Fail<F, Func<T2, S>>(f1)),
            map => source.Match(
                f2 => Fail<F, Func<T2, S>>(f2),
                x => Succeed<F, Func<T2, S>>(y => map(x, y))));
    }

    public static Validated<F, Func<T2, T3, S>> Apply<F, T1, T2, T3, S>(
        this Validated<F, Func<T1, T2, T3, S>> selector,
        Validated<F, T1> source,
        Func<F, F, F> combine)
    {
        if (selector is null)
            throw new ArgumentNullException(nameof(selector));

        return selector.Match(
            f1 => source.Match(
                f2 => Fail<F, Func<T2, T3, S>>(combine(f1, f2)),
                _ => Fail<F, Func<T2, T3, S>>(f1)),
            map => source.Match(
                f2 => Fail<F, Func<T2, T3, S>>(f2),
                x => Succeed<F, Func<T2, T3, S>>((y, z) => map(x, y, z))));
    }

    public static Validated<F, Func<T2, T3, S>> Apply<F, T1, T2, T3, S>(
        this Func<T1, T2, T3, S> map,
        Validated<F, T1> source,
        Func<F, F, F> combine)
    {
        return Apply(
            Succeed<F, Func<T1, T2, T3, S>>((x, y, z) => map(x, y, z)),
            source,
            combine);
    }
}