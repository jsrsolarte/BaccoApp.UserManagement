using Ardalis.Specification;

namespace BaccoApp.UserManagement.Infrastructure.Specification;

public class SpecificationEvaluator : ISpecificationEvaluator
{
    private readonly List<IEvaluator> _evaluators = new();
    public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();

    public SpecificationEvaluator()
    {
        _evaluators.AddRange(new IEvaluator[]
        {
            WhereEvaluator.Instance,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance
        });
    }

    public SpecificationEvaluator(IEnumerable<IEvaluator> evaluators)
    {
        _evaluators.AddRange(evaluators);
    }

    public virtual IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> inputQuery,
        ISpecification<T, TResult> specification) where T : class
    {
        inputQuery = GetQuery(inputQuery, (ISpecification<T>)specification);

        return inputQuery.Select(specification.Selector ?? throw new ArgumentNullException(nameof(specification)));
    }

    public virtual IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification,
        bool evaluateCriteriaOnly = false) where T : class
    {
        var selectedEvaluators = evaluateCriteriaOnly ? _evaluators.Where(x => x.IsCriteriaEvaluator) : _evaluators;

        return selectedEvaluators.Aggregate(inputQuery,
            (current, evaluator) => evaluator.GetQuery(current, specification));
    }
}