import MusicUploadForm from "./MusicUploadForm";
import MusicList from "./MusicsList";

const MainPage = () => {
    return (
        <div style={{ backgroundColor: "#121212", color: "white", minHeight: "100vh", padding: "2rem", fontFamily: "sans-serif" }}>
            <MusicUploadForm />
            <div style={{ marginTop: "1rem" }}>
                <MusicList />
            </div>
        </div>
    );
};

export default MainPage;
