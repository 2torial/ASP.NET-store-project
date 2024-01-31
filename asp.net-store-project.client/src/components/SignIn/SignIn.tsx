import './SignIn.css';
function SignIn() {
	return <main className="sign-in">
		<form className="account-form">
			<div className="title-section">
				<h2>Sign in</h2>
			</div>
			<div className="input-section">
				<label htmlFor="username">Username</label>
				<input id="username" type="text" name="UserName" />
			</div>
			<div className="input-section">
				<label htmlFor="password">Password</label>
				<input id="password" type="password" name="PassWord" />
			</div>
			<div className="submit-section">
				<input type="submit" value="Log in" />
			</div>
		</form>
	</main>;
}

export default SignIn;