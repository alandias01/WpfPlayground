using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlayground.DesignPatterns.Unorganized
{
    public class ValidationPattern
    {
        /*
         * Encapsulation: Keeps validation logic separate from the business logic.
         * Extensibility: Easily add new validators without modifying existing code.
         * Testability: Each validator can be unit tested independently.
         * Reusability: Validators can be reused across different parts of the application.
         */
        public ValidationPattern()
        {
            var user = new User() { Name = "Alan", Age = 1 };

            //use by itself
            var result = new UserValidator().IsValid(user);

            //using multiple validators                        
            var validators = new List<IValidator<User>>() { new UserValidator(), new DrinkingAgeValidator() };
            var compositeValidator = new CompositeValidator<User>(validators);
            result = compositeValidator.IsValid(user);

            /* Factory
             * The purpose of using factory is you do setup somewhere else, then pass that to 
             * a section where the focus is running the validation, not focused on validator creation
             * This allows you to modify creation outside the area focused on running the validation             
             */
            var vFac = new ValidationFactory<User>();
            vFac.RegisterValidator(new UserValidator());
            vFac.RegisterValidator(new DrinkingAgeValidator());
            ValidationUsingFactory(vFac);
        }

        //Focus is on user and running the validation, not how the validator is constructed
        public void ValidationUsingFactory(ValidationFactory<User> validationFactory)
        {
            var user = new User() { Name = "Alan", Age = 1 };
            var compositeValidator = validationFactory.CreateCompositeValidator();
            compositeValidator.IsValid(user);
        }
    }

    public interface IValidator<T>
    {
        bool IsValid(T entity);
    }

    //Validator
    public class UserValidator : IValidator<User>
    {
        public bool IsValid(User entity) => !string.IsNullOrEmpty(entity.Name) && entity.Age > 0;
    }

    //Validator
    public class DrinkingAgeValidator : IValidator<User>
    {
        public bool IsValid(User entity) => !string.IsNullOrEmpty(entity.Name) && entity.Age >= 21;
    }

    //Validator if multiple steps are needed
    //Responsible for grouping multiple validators together and ensuring
    //they all validate an entity before deeming it valid
    //They aggregate the validators
    public class CompositeValidator<T> : IValidator<T>
    {
        private readonly List<IValidator<T>> _validators;
        public CompositeValidator(IEnumerable<IValidator<T>> validators)
        {
            _validators = validators.ToList();
        }

        public bool IsValid(T entity)
        {
            return _validators.All(v => v.IsValid(entity));
        }
    }

    //Using a factory
    //When you want to manage validator creation separately
    //When you need flexibility adding new validators without modifying existing code

    public class ValidationFactory<T>
    {
        private readonly List<IValidator<T>> _validators = new List<IValidator<T>>();

        public void RegisterValidator(IValidator<T> validator) => _validators.Add(validator);
        public IValidator<T> CreateCompositeValidator() => new CompositeValidator<T>(_validators);
    }

    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

}
