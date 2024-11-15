using System.Windows;
using System.Windows.Input;
using DataAccessLayer;

namespace ClientChat
{
    public partial class NicknameWindow : Window
    {
        private readonly Repository _repository;

        public string Nickname { get; private set; }
        public string Password { get; private set; }

        public NicknameWindow()
        {
            InitializeComponent();
            _repository = new Repository(new ClientChatContext());
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Nickname = nicknameTextBox.Text;
            Password = passwordTextBox.Password;

            if (string.IsNullOrEmpty(Nickname))
            {
                MessageBox.Show("Будь ласка, введіть дійсне ім'я.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Перевірка, чи користувач з таким ім'ям вже існує
            if (_repository.IsNicknameTaken(Nickname))
            {
                // Якщо користувач з таким ім'ям є, перевіряємо пароль
                if (_repository.AuthenticateClient(Nickname, Password))
                {
                    // Якщо пароль збігається, пропускаємо користувача
                    MessageBox.Show("Вхід успішний!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                    return;
                }
                else
                {
                    // Якщо пароль не збігається, виводимо повідомлення
                    MessageBox.Show("Користувач з таким іменем вже існує, але пароль невірний.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // Перевірка валідності пароля для нової реєстрації
            if (!_repository.IsPasswordValid(Password))
            {
                MessageBox.Show("Пароль повинен містити щонайменше 8 символів, включати букви та цифри і не мати пробілів.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Якщо користувача з таким ім'ям немає, реєструємо нового
            bool registrationSuccessful = _repository.RegisterClient(Nickname, Password);
            if (registrationSuccessful)
            {
                MessageBox.Show("Реєстрація успішна!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Не вдалося зареєструвати користувача.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
