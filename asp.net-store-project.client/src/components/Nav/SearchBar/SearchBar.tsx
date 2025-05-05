import { useRef, useState } from "react";
import "./SearchBar.css";
import { useNavigate } from "react-router-dom";

function SearchBar() {
    const searchBarRef = useRef(null);
    const focusSearchBar = () => (searchBarRef.current! as HTMLElement).classList.add("focused");
    const unfocusSearchBar = () => (searchBarRef.current! as HTMLElement).classList.remove("focused");
    const [searchBarInput, setSearchBar] = useState("");
    const navigate = useNavigate();

    const search = (event: React.SyntheticEvent) => {
        event.preventDefault();
        const input = searchBarInput
            .trim()
            .replace("#", "")
            .replace(/\s+/g, "+")
            .toString();
        navigate({
            pathname: "/store",
            search: `?search=${input}`,
        });
    };

    return <div className="search-bar-section">
        <form autoComplete="off" onSubmit={search}>
            <search className="search-bar" ref={searchBarRef} onFocus={focusSearchBar} onBlur={unfocusSearchBar}>
                <input className="input-area" type="text" name="SearchBar" placeholder="Search"
                    value={searchBarInput} onChange={e => setSearchBar(e.target.value)} />
                <input className="search-button fa" type="submit" value="&#xf002;" />
            </search>
        </form>
    </div>;
}

export default SearchBar;