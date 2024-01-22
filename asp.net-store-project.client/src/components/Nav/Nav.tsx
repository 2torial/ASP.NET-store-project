import './Nav.css';

export function Nav() {
  	return <nav>
		<div className="navigation">
			<div className="logo-section">
				<input id="logo" type="image" src="https://placehold.co/120x50" alt="logo" />
			</div>
			<div className="empty-space-section"></div>
			<div className="search-bar-section">
				<form autoComplete="off">
					<div className="search-bar">
						<input className="input-area" type="text" name="SearchBar" placeholder="Search" />
						<input className="search-button" type="image" src="https://placehold.co/40x40" id="search-link" alt="magnifier" />
					</div>
				</form>
			</div>
			<div className="menu-section">
				<input id="basket-link" type="image" src="https://placehold.co/40x40" alt="basket" />
				<input id="profile-link" type="image" src="https://placehold.co/40x40" alt="profile" />
			</div>
		</div>
		<div className="menu">
			<div className="empty-space"></div>
			<div className="option-section">
				<input id="sign-in-link" type="button" value="Sign in" />
			</div>
			<div className="option-section">
				<input id="sign-up-link" type="button" value="Sign up" />
			</div>
		</div>
	</nav>;
}

export default Nav;