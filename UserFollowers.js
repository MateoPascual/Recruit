import React from "react";
import { user_followers, users_getById } from "./server";
import "./UserFollowers.css";

class UserFollowers extends React.Component {
  state = {
    data: [],
    flag: false
  };

  componentDidMount() {
    const data = {};
    data.Id = this.props.id;
    data.GetShortList = false;
    let promises = [];
    //GETSHORTLIST IS NEEDED TO FLIP THE SQL COMMAND IN C# THE DATA DOES NOT GO TO THE DATABASE TRUE FOR SHORTLIST AND FALSE FOR ALL FOLLOWERS
    const myPromise = user_followers(data);
    const newPromise = myPromise.then(resp => {
      this.setState({ data: resp.data.items });
    });
    promises.push(newPromise);
    Promise.all(promises).then(() => {
      this.setState({ flag: true });
    });
  }

  render() {
    return (
      <React.Fragment>
        {this.state.flag ? (
          <Followers array={this.state.data} />
        ) : (
          <p>LOADING...</p>
        )}
      </React.Fragment>
    );
  }
}

export default UserFollowers;

function Followers(props) {
  let array1 = props.array;
  if (array1.length === 0) {
    return (
      <div>
        <center>
          <p>No Followers</p>
        </center>
      </div>
    );
  } else {
    let mapper = array1.map(array => (
      <div key={array.receiverId} className="panel-body body">
        <div className="media">
          <div className="img-circular pull-left">
            <img src={array.avatarUrl} />
          </div>
          <div className="media-body">
            <a href="#" className="text-success">
              {array.firstName} {array.lastName}
            </a>
            <p>{array.type}</p>
            <small className="block">Followers: {array.followers}</small>
          </div>
        </div>
      </div>
    ));
    return <React.Fragment>{mapper}</React.Fragment>;
  }
}
