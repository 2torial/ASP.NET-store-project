import './SignUp.css';

export function SignUp() {
    return <form className="account-form">
		<div className="title-section">
			<h2>Sign up</h2>
		</div>
		<div className="input-section">
			<label htmlFor="username">Enter username</label>
			<input id="username" type="text" name="UserName" />
		</div>
		<div className="input-section">
			<label htmlFor="password">Enter password</label>
			<input id="password" type="password" name="PassWord" />
		</div>
		<div className="input-section">
			<label htmlFor="password-repeat">Repeat your password</label>
			<input id="password-repeat" type="password" name="PassWordRepeat" />
		</div>
		<div className="submit-section">
			<input type="submit" value="Register" />
		</div>
	</form>;
}

export default SignUp;