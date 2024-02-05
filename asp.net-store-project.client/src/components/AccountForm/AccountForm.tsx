import { useNavigate } from 'react-router-dom';
import { FormID, collectData } from '../../shared/FormDataCollection';
import './AccountForm.css';

interface AccountFormProps {
	newAccount: boolean;
}

function AccountForm({ newAccount }: AccountFormProps) {
	const navigate = useNavigate();

	const signIn = async (event: React.SyntheticEvent) => {
		event.preventDefault();
		const data = collectData(FormID.SignIn);
		const response = await fetch('/api/account/create', {
			method: "post",
			body: data
		});
		const result: object = await response.json();
		console.log(result);
		navigate("/");
	};
	const signUp = async (event: React.SyntheticEvent) => {
		event.preventDefault();
		const data = collectData(FormID.SignUp);
		for (const value of data.values()) {
			if (value === "") {
				alert("All sections are required!");
				return;
			}
		}
		if (data.get("PassWord") !== data.get("PassWordRepeat")) {
			alert("Passwords must match!");
			return;
		}
		data.delete("PassWordRepeat");
		const response = await fetch('/api/account/create', {
			method: "post",
			body: data
		});
		const result: object = await response.json();
		console.log(result);
		navigate("/");
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