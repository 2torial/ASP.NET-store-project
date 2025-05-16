export const collectData = (...ids: string[]): FormData => {
	let data = undefined;
	for (const id of ids) {
		if (id == "") continue;
		const form: HTMLFormElement | null = document.querySelector(`form#${id}`);
		if (form !== null) {
			if (data === undefined) data = new FormData(form);
			else for (const [name, value] of new FormData(form).entries()) {
				data.append(name, value);
			}
		}
	}
	return data !== undefined ? data : new FormData();
};

export enum FormID {
	Basket = "basket",
	Filters = "filters",
	Settings = "settings",
	SignIn = "sign-in",
	SignUp = "sign-up",
	Summary = "summary",
	Nil = ""
}