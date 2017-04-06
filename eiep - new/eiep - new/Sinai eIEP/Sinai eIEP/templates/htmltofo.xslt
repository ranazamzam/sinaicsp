﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:fo="http://www.w3.org/1999/XSL/Format"
                version="1.0">

  <xsl:output method="xml" indent="yes"/>
  <xsl:variable name="pagewidth" select="21.5"/>
  <xsl:variable name="bodywidth" select="19"/>
  <xsl:template match="html">
    <fo:root xmlns:fo="http://www.w3.org/1999/XSL/Format">

      <fo:layout-master-set>
        <fo:simple-page-master master-name="mainPage"
          page-height="27.9cm"
          page-width="{$pagewidth}cm"
          margin-left="1cm"
          margin-right="1cm"
          margin-top="1cm"
          margin-bottom="1cm">
          <fo:region-before extent="1cm"/>
          <fo:region-after extent="1cm"/>
          <fo:region-body
            margin-top="1cm"
            margin-bottom="1cm" />
        </fo:simple-page-master>

      </fo:layout-master-set>

      <fo:page-sequence master-reference="mainPage" initial-page-number="1">
        <xsl:apply-templates />
      </fo:page-sequence>
    </fo:root>
  </xsl:template>

  <xsl:template match="head/title">
    <fo:static-content flow-name="xsl-region-before">
      <fo:block font-family="serif" font-size="10pt"
        text-align="center">
        <xsl:value-of select="."/>
      </fo:block>
    </fo:static-content>

    <fo:static-content flow-name="xsl-region-after">
      <fo:block font-family="serif" font-size="10pt"
        text-align="center">
        Page <fo:page-number />
      </fo:block>
    </fo:static-content>
  </xsl:template>

  <xsl:template match="body">
    <fo:flow flow-name="xsl-region-body" font-family="serif"
      font-size="10pt">
      <xsl:apply-templates/>
    </fo:flow>
  </xsl:template>

  <xsl:template match="blockquote">
    <fo:block
      space-before="6pt" space-after="6pt"
      margin-left="2em" margin-right="2em">
      <xsl:call-template name="set-pagebreak"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="h1">
    <fo:block font-size="18pt">
      <xsl:call-template name="set-alignment"/>
      <xsl:call-template name="set-pagebreak"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="h2">
    <fo:block font-size="14pt">
      <xsl:call-template name="set-alignment"/>
      <xsl:call-template name="set-pagebreak"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="h3">
    <fo:block font-size="12pt" font-weight="bold"
      space-before="6pt">
      <xsl:call-template name="set-alignment"/>
      <xsl:call-template name="set-pagebreak"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="h4">
    <fo:block font-weight="bold" space-before="1mm">
      <xsl:call-template name="set-alignment"/>
      <xsl:call-template name="set-pagebreak"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template name="set-alignment">
    <xsl:choose>
      <xsl:when test="@align='left' or contains(@class,'left')">
        <xsl:attribute name="text-align">start</xsl:attribute>
      </xsl:when>
      <xsl:when test="@align='center' or contains(@class,'center')">
        <xsl:attribute name="text-align">center</xsl:attribute>
      </xsl:when>
      <xsl:when test="@align='right' or contains(@class,'right')">
        <xsl:attribute name="text-align">end</xsl:attribute>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="set-pagebreak">
    <xsl:if test="contains(@style, 'page-break-before')">
      <xsl:attribute name="break-before">page</xsl:attribute>
    </xsl:if>
    <xsl:if test="contains(@style, 'page-break-after')">
      <xsl:attribute name="break-after">page</xsl:attribute>
      <xsl:message>
        <xsl:value-of select="@style"/>
      </xsl:message>
    </xsl:if>
  </xsl:template>

  <xsl:template match="div">
    <fo:block>
      <xsl:if test="@class='bordered'">
        <xsl:attribute name="border-width">1pt</xsl:attribute>
        <xsl:attribute name="border-style">solid</xsl:attribute>
      </xsl:if>
      <xsl:call-template name="set-alignment"/>
      <xsl:call-template name="set-pagebreak"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="ol">
    <fo:list-block provisional-distance-between-starts="10mm"
      provisional-label-separation="5mm"
      space-before="6pt" space-after="6pt">
      <xsl:call-template name="set-pagebreak"/>
      <xsl:apply-templates/>
    </fo:list-block>
  </xsl:template>

  <xsl:template match="ol/li">
    <xsl:variable name="indention"
      select="5*count(ancestor::ol)"/>
    <fo:list-item space-after="1mm">
      <fo:list-item-label start-indent="{$indention}mm" end-indent="label-end()">
        <fo:block>
          <xsl:choose>
            <xsl:when test="../@type != ''">
              <xsl:number format="{../@type}"/>.
            </xsl:when>
            <xsl:otherwise>
              <xsl:number format="1"/>.
            </xsl:otherwise>
          </xsl:choose>
        </fo:block>
      </fo:list-item-label>
      <fo:list-item-body start-indent="body-start()">
        <fo:block>
          <xsl:apply-templates/>
        </fo:block>
      </fo:list-item-body>
    </fo:list-item>
  </xsl:template>

  <xsl:template match="ul">
    <fo:list-block provisional-distance-between-starts="3mm"
      provisional-label-separation="1mm"
      space-before="6pt" space-after="6pt">
      <xsl:call-template name="set-pagebreak"/>
      <xsl:apply-templates/>
    </fo:list-block>
  </xsl:template>

  <xsl:template match="ul/li">
    <fo:list-item space-after="1mm">
      <fo:list-item-label start-indent="5mm">
        <xsl:choose>
          <xsl:when test="../@type ='disc'">
            <fo:block>&#x2022;</fo:block>
          </xsl:when>
          <xsl:when test="../@type='square'">
            <fo:block font-family="ZapfDingbats">&#110;</fo:block>
          </xsl:when>
          <xsl:when test="../@type='circle'">
            <fo:block font-family="ZapfDingbats">&#109;</fo:block>
          </xsl:when>
          <xsl:otherwise>
            <xsl:choose>
              <xsl:when test="count(ancestor::ul) = 1">
                <fo:block>&#x2022;</fo:block>
              </xsl:when>
              <xsl:when test="count(ancestor::ul) = 2">
                <fo:block font-family="ZapfDingbats">&#109;</fo:block>
              </xsl:when>
              <xsl:otherwise>
                <fo:block font-family="ZapfDingbats">&#110;</fo:block>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:otherwise>
        </xsl:choose>
      </fo:list-item-label>
      <fo:list-item-body start-indent="body-start()">
        <fo:block>
          <xsl:apply-templates/>
        </fo:block>
      </fo:list-item-body>
    </fo:list-item>
  </xsl:template>

  <xsl:template match="dl">
    <fo:block>
      <xsl:call-template name="set-pagebreak"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="dt">
    <fo:block space-before="0.5em">
      <xsl:call-template name="set-pagebreak"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="dd">
    <fo:block space-after="0.25em" start-indent="5mm">
      <xsl:call-template name="set-pagebreak"/>
      <xsl:call-template name="set-pagebreak"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="table">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="caption">
    <fo:caption>
      <fo:block>
        <xsl:apply-templates />
      </fo:block>
    </fo:caption>
  </xsl:template>

  <xsl:template match="tbody">
    <fo:table space-before="0.5em" space-after="0.5em" table-layout="fixed">
      <xsl:for-each select="tr[1]/th|tr[1]/td">
        <xsl:variable name="width">
          <xsl:call-template name="find-width">
            <xsl:with-param name="node" select="."/>
          </xsl:call-template>
        </xsl:variable>
        <fo:table-column>
          <xsl:attribute name="column-width">
            <xsl:choose>
              <xsl:when test="contains($width, '%')">
                <xsl:value-of
				select="floor(number(translate($width,'%','')) div 100
					* $bodywidth)"/>cm
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of
   select="$width div 72"/>in
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </fo:table-column>
      </xsl:for-each>
      <fo:table-body>
        <xsl:apply-templates />
      </fo:table-body>
    </fo:table>
  </xsl:template>

  <xsl:template match="tr">
    <fo:table-row>
      <xsl:if test="@space-before != ''">
        <xsl:attribute name="space-before">
          <xsl:value-of
			select="@space-before" />
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="@class='graybar'">
        <xsl:attribute name="background-color">#ddd</xsl:attribute>
      </xsl:if>
      <xsl:apply-templates/>
    </fo:table-row>
  </xsl:template>

  <xsl:template match="th">
    <fo:table-cell font-weight="bold" text-align="center">
      <xsl:if test="ancestor::table/@border &gt; 0">
        <xsl:attribute name="border-style">solid</xsl:attribute>
        <xsl:attribute name="border-width">1pt</xsl:attribute>
      </xsl:if>
      <fo:block>
        <xsl:apply-templates/>
      </fo:block>
    </fo:table-cell>
  </xsl:template>

  <xsl:template match="td">
    <fo:table-cell>
      <xsl:if test="ancestor::table/@border &gt; 0">
        <xsl:attribute name="border-style">solid</xsl:attribute>
        <xsl:attribute name="border-width">1pt</xsl:attribute>
      </xsl:if>
      <fo:block>
        <xsl:call-template name="set-alignment"/>
        <xsl:apply-templates/>
      </fo:block>
    </fo:table-cell>
  </xsl:template>

  <xsl:template match="tt">
    <fo:inline font-family="monospace">
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <xsl:template name="find-width">
    <xsl:param name="node"/>
    <xsl:choose>
      <xsl:when test="@width">
        <xsl:value-of select="@width"/>
      </xsl:when>
      <xsl:when test="@style">
        <xsl:value-of select="substring-before(
				substring-after(@style, ':'), ';')"/>
      </xsl:when>
      <xsl:otherwise>0</xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="code">
    <fo:inline font-family="monospace">
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <xsl:template match="img">
    <fo:external-graphic>
      <xsl:attribute name="src">
        file:<xsl:value-of
		select="@src"/>
      </xsl:attribute>
      <xsl:attribute name="width">
        <xsl:value-of
		select="@width"/>px
      </xsl:attribute>
      <xsl:attribute name="height">
        <xsl:value-of
		select="@height"/>px
      </xsl:attribute>
    </fo:external-graphic>
  </xsl:template>

  <xsl:template match="pre">
    <fo:block white-space-collapse="false" font-family="monospace" font-size="10pt">
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="b|strong">
    <fo:inline font-weight="bold">
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <xsl:template match="i|em">
    <fo:inline font-style="italic">
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <xsl:template match="u">
    <fo:inline text-decoration="underline">
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <xsl:template match="span[@style and contains(@style, 'text-decoration: underline;')]">
    <fo:inline text-decoration="underline">
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>
  
  <xsl:template match="hr">
    <fo:block>
      <fo:leader
        leader-pattern="rule" leader-length.optimum="100%"
        rule-style="double" rule-thickness="1pt"/>
    </fo:block>
  </xsl:template>

  <xsl:template match="br">
    <fo:block space-before="2.5mm">
      <xsl:text>&#x0D;&#x0A;</xsl:text>
    </fo:block>
  </xsl:template>

  <xsl:template match="div[@id='top-navig']"/>
</xsl:stylesheet>