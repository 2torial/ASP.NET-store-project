import './AccountForm.css';

interface AccountFormProps {
	newAccount: boolean;
}

function AccountForm({ newAccount }: AccountFormProps) {
	return <main id="account-form">
		<form className="account-form">
			<div className="title-section">
				<h2>{newAccount ? "Sign up" : "Sign in"}</h2>
			</div>
			<div className="input-section">
				<label htmlFor="username">{newAccount ? "Enter username" : "Username"}</label>
				<input id="username" type="text" name="UserName" />
			</div>
			<div className="input-section">
				<label htmlFor="password">{newAccount ? "Enter password" : "Password"}</label>
				<input id="password" type="password" name="PassWord" />
			</div>
			{newAccount
				? <div className="input-section">
					<label htmlFor="password-repeat">Retype your password</label>
					<input id="password-repeat" type="password" name="PassWordRepeat" />
				</div>
				: <></>
			}
			<div className="submit-section">
				<input type="submit" value={newAccount ? "Register" : "Log in"} />
			</div>
		</form>
	</main>;
}

export default AccountForm;