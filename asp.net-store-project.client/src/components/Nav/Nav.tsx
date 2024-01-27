import { useRef } from 'react';
import './Nav.css';

export function Nav() {
	const searchBarRef = useRef(null);
	const focusSearchBar = () => (searchBarRef.current! as HTMLElement).classList.add("focused");
	const unfocusSearchBar = () => (searchBarRef.current! as HTMLElement).classList.remove("focused");

	const menuRef = useRef(null);
	const openMenu = () => (menuRef.current! as HTMLElement).style.top = "60px";
	const hideMenu = () => (menuRef.current! as HTMLElement).style.top = "7px";

    return <nav onMouseLeave={hideMenu}>
		<div className="navigation">
			<div className="logo-section">
				<input id="logo" type="image" src="https://placehold.co/120x50" alt="logo" />
			</div>
			<div className="empty-space-section">
				
			</div>
			<div className="search-bar-section">
				<form autoComplete="off">
					<search className="search-bar" ref={searchBarRef} onFocus={focusSearchBar} onBlur={unfocusSearchBar}>
						<input className="input-area" type="text" name="SearchBar" placeholder="Search" />
						<input className="search-button" type="image" src="https://placehold.co/40x40" id="search-link" alt="magnifier" />
					</search>
				</form>
			</div>
			<div className="menu-section">
				<input type="image" src="https://placehold.co/40x40" alt="basket" />
				<input type="image" onMouseOver={openMenu} src="https://placehold.co/40x40" alt="profile" />
			</div>
		</div>
		<div className="menu" ref={menuRef}>
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