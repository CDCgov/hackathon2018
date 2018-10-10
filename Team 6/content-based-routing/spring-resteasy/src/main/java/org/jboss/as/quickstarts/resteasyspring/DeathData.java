package org.jboss.as.quickstarts.resteasyspring;

import org.codehaus.jackson.annotate.JsonIgnoreProperties;
import org.codehaus.jackson.annotate.JsonProperty;
import org.json.JSONObject;

@JsonIgnoreProperties(ignoreUnknown = true)
public class DeathData {
    private String sex;
    private String age;
    private String placdth;
    private String marstat;
    private String weekday;
    private String year;
    private String injwork;
    private String mandeath;
    private String autopsy;
    private String ucod;
    private String ranum;
    private String record_1;
    private String record_2;
    private String record_3;
    private String record_4;
    private String record_5;
    private String record_6;
    private String record_7;
    private String record_8;
    private String record_9;
    private String record_10;
    private String record_11;
    private String record_12;
    private String record_13;
    private String record_14;
    private String record_15;
    private String record_16;
    private String record_17;
    private String record_18;
    private String record_19;
    private String record_20;
    private String race;

    @JsonProperty("Place of Injury")
    private String Place_of_Injury;

    @JsonProperty("Cause of Death A")
    private String Cause_of_Death_A;

    @JsonProperty("Cause of Death B")
    private String Cause_of_Death_B;

    @JsonProperty("Cause of Death C")
    private String Cause_of_Death_C;

    @JsonProperty("Cause of Death D")
    private String Cause_of_Death_D;

    @JsonProperty("Other Data on Death Certificate")
    private String Other_Data_on_Death_Certificate;

    @Override
    public String toString() {
        return "DeathData{" +
                "sex='" + sex + '\'' +
                ", age='" + age + '\'' +
                ", placdth='" + placdth + '\'' +
                ", marstat='" + marstat + '\'' +
                ", weekday='" + weekday + '\'' +
                ", year='" + year + '\'' +
                ", injwork='" + injwork + '\'' +
                ", mandeath='" + mandeath + '\'' +
                ", autopsy='" + autopsy + '\'' +
                ", ucod='" + ucod + '\'' +
                ", ranum='" + ranum + '\'' +
                ", record_1='" + record_1 + '\'' +
                ", record_2='" + record_2 + '\'' +
                ", record_3='" + record_3 + '\'' +
                ", record_4='" + record_4 + '\'' +
                ", record_5='" + record_5 + '\'' +
                ", record_6='" + record_6 + '\'' +
                ", record_7='" + record_7 + '\'' +
                ", record_8='" + record_8 + '\'' +
                ", record_9='" + record_9 + '\'' +
                ", record_10='" + record_10 + '\'' +
                ", record_11='" + record_11 + '\'' +
                ", record_12='" + record_12 + '\'' +
                ", record_13='" + record_13 + '\'' +
                ", record_14='" + record_14 + '\'' +
                ", record_15='" + record_15 + '\'' +
                ", record_16='" + record_16 + '\'' +
                ", record_17='" + record_17 + '\'' +
                ", record_18='" + record_18 + '\'' +
                ", record_19='" + record_19 + '\'' +
                ", record_20='" + record_20 + '\'' +
                ", race='" + race + '\'' +
                ", Place_of_Injury='" + Place_of_Injury + '\'' +
                ", Cause_of_Death_A='" + Cause_of_Death_A + '\'' +
                ", Cause_of_Death_B='" + Cause_of_Death_B + '\'' +
                ", Cause_of_Death_C='" + Cause_of_Death_C + '\'' +
                ", Cause_of_Death_D='" + Cause_of_Death_D + '\'' +
                ", Other_Data_on_Death_Certificate='" + Other_Data_on_Death_Certificate + '\'' +
                '}';
    }

    public String getSex() {
        return sex;
    }

    public void setSex(String sex) {
        this.sex = sex;
    }

    public String getAge() {
        return age;
    }

    public void setAge(String age) {
        this.age = age;
    }

    public String getPlacdth() {
        return placdth;
    }

    public void setPlacdth(String placdth) {
        this.placdth = placdth;
    }

    public String getMarstat() {
        return marstat;
    }

    public void setMarstat(String marstat) {
        this.marstat = marstat;
    }

    public String getWeekday() {
        return weekday;
    }

    public void setWeekday(String weekday) {
        this.weekday = weekday;
    }

    public String getYear() {
        return year;
    }

    public void setYear(String year) {
        this.year = year;
    }

    public String getInjwork() {
        return injwork;
    }

    public void setInjwork(String injwork) {
        this.injwork = injwork;
    }

    public String getMandeath() {
        return mandeath;
    }

    public void setMandeath(String mandeath) {
        this.mandeath = mandeath;
    }

    public String getAutopsy() {
        return autopsy;
    }

    public void setAutopsy(String autopsy) {
        this.autopsy = autopsy;
    }

    public String getUcod() {
        return ucod;
    }

    public void setUcod(String ucod) {
        this.ucod = ucod;
    }

    public String getRanum() {
        return ranum;
    }

    public void setRanum(String ranum) {
        this.ranum = ranum;
    }

    public String getRecord_1() {
        return record_1;
    }

    public void setRecord_1(String record_1) {
        this.record_1 = record_1;
    }

    public String getRecord_2() {
        return record_2;
    }

    public void setRecord_2(String record_2) {
        this.record_2 = record_2;
    }

    public String getRecord_3() {
        return record_3;
    }

    public void setRecord_3(String record_3) {
        this.record_3 = record_3;
    }

    public String getRecord_4() {
        return record_4;
    }

    public void setRecord_4(String record_4) {
        this.record_4 = record_4;
    }

    public String getRecord_5() {
        return record_5;
    }

    public void setRecord_5(String record_5) {
        this.record_5 = record_5;
    }

    public String getRecord_6() {
        return record_6;
    }

    public void setRecord_6(String record_6) {
        this.record_6 = record_6;
    }

    public String getRecord_7() {
        return record_7;
    }

    public void setRecord_7(String record_7) {
        this.record_7 = record_7;
    }

    public String getRecord_8() {
        return record_8;
    }

    public void setRecord_8(String record_8) {
        this.record_8 = record_8;
    }

    public String getRecord_9() {
        return record_9;
    }

    public void setRecord_9(String record_9) {
        this.record_9 = record_9;
    }

    public String getRecord_10() {
        return record_10;
    }

    public void setRecord_10(String record_10) {
        this.record_10 = record_10;
    }

    public String getRecord_11() {
        return record_11;
    }

    public void setRecord_11(String record_11) {
        this.record_11 = record_11;
    }

    public String getRecord_12() {
        return record_12;
    }

    public void setRecord_12(String record_12) {
        this.record_12 = record_12;
    }

    public String getRecord_13() {
        return record_13;
    }

    public void setRecord_13(String record_13) {
        this.record_13 = record_13;
    }

    public String getRecord_14() {
        return record_14;
    }

    public void setRecord_14(String record_14) {
        this.record_14 = record_14;
    }

    public String getRecord_15() {
        return record_15;
    }

    public void setRecord_15(String record_15) {
        this.record_15 = record_15;
    }

    public String getRecord_16() {
        return record_16;
    }

    public void setRecord_16(String record_16) {
        this.record_16 = record_16;
    }

    public String getRecord_17() {
        return record_17;
    }

    public void setRecord_17(String record_17) {
        this.record_17 = record_17;
    }

    public String getRecord_18() {
        return record_18;
    }

    public void setRecord_18(String record_18) {
        this.record_18 = record_18;
    }

    public String getRecord_19() {
        return record_19;
    }

    public void setRecord_19(String record_19) {
        this.record_19 = record_19;
    }

    public String getRecord_20() {
        return record_20;
    }

    public void setRecord_20(String record_20) {
        this.record_20 = record_20;
    }

    public String getRace() {
        return race;
    }

    public void setRace(String race) {
        this.race = race;
    }

    public String getPlace_of_Injury() {
        return Place_of_Injury;
    }

    public void setPlace_of_Injury(String place_of_Injury) {
        Place_of_Injury = place_of_Injury;
    }

    public String getCause_of_Death_A() {
        return Cause_of_Death_A;
    }

    public void setCause_of_Death_A(String cause_of_Death_A) {
        Cause_of_Death_A = cause_of_Death_A;
    }

    public String getCause_of_Death_B() {
        return Cause_of_Death_B;
    }

    public void setCause_of_Death_B(String cause_of_Death_B) {
        Cause_of_Death_B = cause_of_Death_B;
    }

    public String getCause_of_Death_C() {
        return Cause_of_Death_C;
    }

    public void setCause_of_Death_C(String cause_of_Death_C) {
        Cause_of_Death_C = cause_of_Death_C;
    }

    public String getCause_of_Death_D() {
        return Cause_of_Death_D;
    }

    public void setCause_of_Death_D(String cause_of_Death_D) {
        Cause_of_Death_D = cause_of_Death_D;
    }

    public String getOther_Data_on_Death_Certificate() {
        return Other_Data_on_Death_Certificate;
    }

    public void setOther_Data_on_Death_Certificate(String other_Data_on_Death_Certificate) {
        Other_Data_on_Death_Certificate = other_Data_on_Death_Certificate;
    }
}
