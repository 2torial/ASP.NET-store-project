import { useNavigate } from 'react-router-dom';
import { FormID, collectData } from '../../shared/FormDataCollection';
import './AccountForm.css';
import { useEffect, useState } from 'react';

interface AccountFormProps {
	updateUserIdentity(): void,
	newAccount: boolean;
}

function AccountForm({ updateUserIdentity, newAccount }: AccountFormProps) {
	const [errors, setErrors] = useState({
		UserName: [],
		PassWord: [],
		RetypedPassWord: [],
	});
	const navigate = useNavigate();

	useEffect(() => setErrors({
		UserName: [],
		PassWord: [],
		RetypedPassWord: [],
	}), [newAccount]);

	const signIn = async (event: React.SyntheticEvent) => {
		event.preventDefault();
		const formData = collectData(FormID.SignIn);

		const response = await fetch('/api/account/login', {
			method: "post",
			body: formData
		});
		if (response.ok) {
			updateUserIdentity();
			navigate("/");
			alert(await response.text());
		} else response.json().then(data => setErrors(data.errors));
	};

	const signUp = async (event: React.SyntheticEvent) => {
		event.preventDefault();
		const formData = collectData(FormID.SignUp);

		const response = await fetch('/api/account/create', {
			method: "post",
			body: formData
		});
		if (response.ok) {
			updateUserIdentity();
			navigate("/");
			alert(await response.text());
		} else response.json().then(data => setErrors(data.errors));
	};

	const formProps = {
		id: newAccount ? FormID.SignUp : FormID.SignIn,
		onSubmit: newAccount ? signUp : signIn,
	}

	return <main id="account-form">
		<form className="account-form" {...formProps} >
			<div className="title-section">
				<h2>{newAccount ? "Sign up" : "Sign in"}</h2>
			</div>
			<div className="input-section">
				<label htmlFor="username">{newAccount ? "Enter username" : "Username"}</label>
				<input id="username" type="text" name="UserName" required />
				{!!errors?.UserName && errors.UserName.map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
			</div>
			<div className="input-section">
				<label htmlFor="password">{newAccount ? "Enter password" : "Password"}</label>
				<input id="password" type="password" name="PassWord" required />
				{!!errors?.PassWord && errors.PassWord.map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
			</div>{newAccount
				? <div className="input-section">
					<label htmlFor="password-repeat">Retype your password</label>
					<input id="password-repeat" type="password" name="RetypedPassWord" required />
					{!!errors?.RetypedPassWord && errors.RetypedPassWord.map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
				</div>
				: <></>}
			<div className="submit-section">
				<input type="submit" value={newAccount ? "Register" : "Log in"} />
			</div>
		</form>
	</main>;
}

export default AccountForm;